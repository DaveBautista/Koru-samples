using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Trigger version of the interactable classes
public class AS_InteractableTrigger : MonoBehaviour
{
    [Header("Trigger Variables")]
    public string TargetTag;

    // If you don't apply a targettag to this target, you will be error spammed.
    private void Start()
    {
        if (TargetTag == null)
        {
            Debug.LogError("You didn't apply a target tag to: " + gameObject.name);
            gameObject.SetActive(false);
        }

        if (GetComponent<SphereCollider>() == null &&
            GetComponent<BoxCollider>() == null &&
            GetComponent<CapsuleCollider>() == null &&
            GetComponent<MeshCollider>() == null)
        {
            Debug.LogError("Why is there no collider on this object: " + gameObject.name);
            gameObject.AddComponent<BoxCollider>();
        }

        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("This triggerbox requires a rigidbody, cmon man");
            Rigidbody body = gameObject.AddComponent<Rigidbody>();
            body.isKinematic = true;
        }

        CheckAttachments();
    }

    public void Verify()
    {
        if (TargetTag == null)
        {
            Debug.LogError("You didn't apply a target tag to: " + gameObject.name);
            gameObject.SetActive(false);
        }

        if (GetComponent<SphereCollider>() == null &&
            GetComponent<BoxCollider>() == null &&
            GetComponent<CapsuleCollider>() == null &&
            GetComponent<MeshCollider>() == null)
        {
            Debug.LogError("Why is there no collider on this object: " + gameObject.name);
            gameObject.AddComponent<BoxCollider>();
        }

        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("This triggerbox requires a rigidbody, cmon man");
            Rigidbody body = gameObject.AddComponent<Rigidbody>();
            body.isKinematic = true;
        }

        CheckAttachments();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("Turn on the trigger on this object: " + gameObject.name);
        if (GetComponent<SphereCollider>() != null)
            GetComponent<SphereCollider>().isTrigger = true;

        if (GetComponent<BoxCollider>() != null)
            GetComponent<BoxCollider>().isTrigger = true;

        if (GetComponent<CapsuleCollider>() != null)
            GetComponent<CapsuleCollider>().isTrigger = true;

        if (GetComponent<MeshCollider>() != null)
            GetComponent<MeshCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TargetTag && !isComplete)
        {
            RegisterAction();
        }
    }

    #region InteractableBaseFunctions/Data
    // This variable does not need to be linked within unity, as it ~should~ be
    // automatic
    private AS_InteractableDirector m_main;
    private AS_InteractableGroup m_group;
    private AS_UIControl ui;

    [Header("Other Variables")]
    [Tooltip(
        "Use this to define which step this object is, " +
        "if the container group is tallying objectives in order. " +
        "Note: This variable is useless on the InteractableBase class")]
    public int m_step = 0;
    public bool m_debug = false;
    public Color m_onColor = Color.red;

    [Header("Additional")]
    public PlayableDirector m_cutscene;
    public ParticleSystem m_particleFX;
    public Animator m_anim;
    public AudioClip m_clip;
    public float m_clipPitch;
    public AudioSource m_audio;
    public GameObject m_obj;

    // Private variables
    private bool complete = false;

    /// <summary>
    /// Check/Set if this 'objective' is complete
    /// </summary>
    public bool isComplete
    {
        get
        {
            return complete;
        }
        set
        {
            complete = value;
        }
    }

    public void RegisterAction()
    {
        Debug.Log("Mini-objective logged with: " + gameObject.name);
        bool keepGoing = true;
        if (m_group != null && m_main == null)
        {
            if (!m_group.completeInOrder)
                m_group.AddCompletion();
            else
                keepGoing = m_group.AddCompletion(m_step);

            if (m_group.PlayCount < m_group.maxCutscenes && m_cutscene != null)
            {
                m_cutscene.Play();
                m_group.PlayCount += 1;
            }
        }
        else if (m_main != null)
        {
            m_main.AddCompletion();
            if (m_main.PlayCount < m_main.maxCutscenes && m_cutscene != null)
            {
                m_cutscene.Play();
                m_main.PlayCount += 1;
            }
        }

        if (!keepGoing)
            return;

        isFXActive = true;
        isComplete = true;
    }

    public void UnregisterAction()
    {
        if (m_group != null && m_main == null)
        {
            m_group.RemoveCompletion();
        }
        else if (m_main != null)
        {
            m_main.RemoveCompletion();
        }

        isFXActive = false;

        isComplete = false;
    }

    public bool isFXActive
    {
        set
        {
            if (value == true)
            {
                if (m_debug)
                    GetComponent<Renderer>().material.color = m_onColor;

                if (m_particleFX != null)
                    m_particleFX.Play();

                if (m_obj != null)
                    m_obj.SetActive(true);

                // If there is an audioclip attached, play it
                if (m_clip != null)
                    gameObject.GetComponent<AudioSource>().pitch = m_clipPitch;
                gameObject.GetComponent<AudioSource>().PlayOneShot(m_clip);

                if (m_anim != null)
                    m_anim.SetTrigger("activate");

                if (m_audio != null)
                    m_audio.Play();
            }
            else
            {
                if (m_debug)
                    GetComponent<Renderer>().material.color = Color.white;

                if (m_particleFX != null)
                    m_particleFX.Stop();

                if (m_obj != null)
                    m_obj.SetActive(false);

                if (m_anim != null)
                    m_anim.SetTrigger("deactivate");

                if (m_audio != null)
                    m_audio.Stop();
            }
        }
    }

    public void PlayClip()
    {
        if (m_clip != null)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(m_clip);
        }
    }

    public AS_InteractableDirector main
    {
        get
        {
            return m_main;
        }
        set
        {
            m_main = value;
        }
    }

    public AS_InteractableGroup group
    {
        get
        {
            return m_group;
        }
        set
        {
            m_group = value;
        }
    }

    public AS_UIControl uiControl
    {
        set
        {
            ui = value;
        }
    }

    public void CheckAttachments()
    {
        if (m_clip != null && GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    public bool isCollectorComplete
    {
        get
        {
            if (m_group != null)
            {
                return m_group.isComplete;
            }
            else
            {
                return m_main.isComplete;
            }
        }
    }
    #endregion
}
