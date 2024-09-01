using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARTrackedImageManager))]
[RequireComponent(typeof(ARPlaneManager))]

public class ImagesMarkerSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] arObjectsToPlace;
    private Vector3 scaleFactor = new Vector3(0.03f, 0.03f, 0.03f);
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    public static Dictionary<string, bool> isImageDetected = new Dictionary<string, bool>();
    public static Dictionary<string, bool> ableToDetect = new Dictionary<string, bool>();
    public static bool[] doneLocation = { false, false, false };
   

    private ARTrackedImageManager m_TrackedImageManager;

    AudioSource audioSource;

    private GameObject ibnBattutaObj;
    [SerializeField] public GameObject ibnBattutaPrefab;

    //  Albalad (1), Diriyah (2), Alula (3)
    public int currentLocation = 0;
    public TextMeshProUGUI locationText;

    public TextMeshProUGUI usernameText;
    public TMP_InputField usernameField;


    public GameObject instructionPanel;
    public GameObject mapPanel;

    public GameObject playButton;
    public GameObject questionButton;
    public GameObject usernameInputField;

    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI questionButtonText;

    public int panelTextCount = 0;
    public List<GameObject> mainPanelText;
    public AudioClip[] instructionSound;

    public int albaladQuesCount = 0;
    public List<GameObject> albaladQuestion;
    public TextMeshProUGUI albaladButtonText;
    public AudioClip[] albaladSound;
    public AudioClip[] albaladSoundAns;

    public int alulaQuesCount = 0;
    public List<GameObject> alulaQuestion;
    public TextMeshProUGUI alulaButtonText;
    public AudioClip[] alulaSound;
    public AudioClip[] alulaSoundAns;

    public int diriyahQuesCount = 0;
    public List<GameObject> diriyahQuestion;
    public TextMeshProUGUI diriyahButtonText;
    public AudioClip[] diriyahSound;
    public AudioClip[] diriyahSoundAns;

    public TextMeshProUGUI scoreNum;
    public static int scoreCount;

    public GameObject[] greenLocation;

    [SerializeField] private ARPlaneManager arPlaneManager;


    void Awake()
    {

        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();

        m_TrackedImageManager.requestedMaxNumberOfMovingImages = 9;

        audioSource = GetComponent<AudioSource>();

        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;


        foreach (GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            arObjects.Add(newARObject.name, newARObject);
            isImageDetected.Add(newARObject.name, false);

            if (newARObject.name == "1-1")
            {
                ableToDetect.Add(newARObject.name, true);
            }

            else
            {
                ableToDetect.Add(newARObject.name, false);
            }
        }
    }

    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        
        if (args.added != null && ibnBattutaObj == null)
        {
            ARPlane arPlane = args.added[0];

            Vector3 position = arPlane.transform.position;
            ibnBattutaObj = Instantiate(ibnBattutaPrefab, position, Quaternion.Euler(0, 180, 0));
            ibnBattutaObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }


    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdatedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdatedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            arObjects[trackedImage.referenceImage.name].SetActive(false);
        }
    }


    private void UpdatedImage(ARTrackedImage trackedImage)
    {
        
        string name = trackedImage.referenceImage.name;

        if (name != null && ableToDetect[name])
        {

            Vector3 position = trackedImage.transform.position;

            if(trackedImage.trackingState == TrackingState.Tracking)
                AssignGameObject(name, position);
            else
            {
                GameObject prefab = arObjects[name];
                prefab.SetActive(false);
            }

        }


    }

    void AssignGameObject(string name, Vector3 newPosition)
    {

        if (arObjectsToPlace != null)
        {
            GameObject prefab = arObjects[name];
            
            prefab.transform.position = newPosition;
            prefab.transform.localScale = scaleFactor;

            prefab.SetActive(true);

            int loc;
            int que;

            bool parseCorr_loc = Int32.TryParse(name.Substring(0, 1), out loc);
            bool parseCorr_que = Int32.TryParse(name.Substring(2, 1), out que);

            // add coins
            if (parseCorr_loc && parseCorr_que && que == 3)
            {
                increaseScore(name, 150);
                doneLocation[loc - 1] = true;
                greenLocation[loc - 1].SetActive(true);
            } else
            {
                increaseScore(name, 50);
            }
                
            foreach (GameObject go in arObjects.Values)
            {
                if (go.name != name)
                {
                    go.SetActive(false);
                }
            }
        }
    }

    public void increaseScore(string name, int amout)
    {
        if (!isImageDetected[name])
        {
            scoreCount += amout;
            scoreNum.text = "" + scoreCount;
        }
        isImageDetected[name] = true;


    }

    public void play()
    {
        //  Displaying the instruction panel
        if (panelTextCount < mainPanelText.Count)
        {

            audioSource.enabled = true;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.PlayOneShot(instructionSound[panelTextCount], 0.5f);


            if (panelTextCount > 0)
            {
                mainPanelText[panelTextCount - 1].SetActive(false);
                
            }

            instructionPanel.SetActive(true);
            mainPanelText[panelTextCount].SetActive(true);

            if (panelTextCount == 1)
                playButtonText.text = "YES!";

            else if (panelTextCount == 4)
            {
                usernameText.text = usernameField.text;

                usernameInputField.SetActive(false);
                playButtonText.text = "Okay";
            }

            else
            {
                playButtonText.text = "Next";
                if (panelTextCount == 3)
                {
                    usernameInputField.SetActive(true);
                }
            }

            //audios[panelTextCount].Play();
            panelTextCount++;

        }
        else
        {
            instructionPanel.SetActive(false);
            mainPanelText[panelTextCount - 1].SetActive(false);
            playButton.SetActive(false);
        }


    }


    // Start is called before the first frame update
    public void question()
    {

        //  Abalad Quesions
        if (currentLocation == 1)
            questionGeneralFunction(ref albaladQuesCount, "1", ref albaladQuestion, ref albaladSound);

        //  Alula Quesions
        else if (currentLocation == 2)
            questionGeneralFunction(ref alulaQuesCount, "2", ref alulaQuestion, ref alulaSound);

        //  Diriyah Quesions
        else if (currentLocation == 3)
            questionGeneralFunction(ref diriyahQuesCount, "3", ref diriyahQuestion, ref diriyahSound);

    }

    public void mapButton()
    {
        if (mapPanel != null)
        {
            bool isActive = mapPanel.activeSelf;
            mapPanel.SetActive(!isActive);
        }
    }

    public void albaladButton()
    {
        locationGeneralButton(1, "Albalad", ref albaladQuestion, ref albaladQuesCount, albaladButtonText, ref albaladSound);
    }

    public void alulaButton()
    {
        locationGeneralButton(2, "Alula", ref alulaQuestion, ref alulaQuesCount, alulaButtonText, ref alulaSound);
    }

    public void diriyahButton()
    {
        locationGeneralButton(3, "Diriyah", ref diriyahQuestion, ref diriyahQuesCount, diriyahButtonText, ref diriyahSound);
    }

    public void questionGeneralFunction(ref int locationCount, string locationNum, ref List<GameObject> locationQuestions, ref AudioClip[] locationSound)
    {
        if (locationCount == 0)
        {
            if (ibnBattutaObj != null)
                ibnBattutaObj.SetActive(false);
            locationQuestions[locationCount].SetActive(false);

            locationCount++;
            locationQuestions[locationCount].SetActive(true);

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.PlayOneShot(locationSound[locationCount], 0.5f);

            ableToDetect[locationNum + "-" + locationCount] = true;
            questionButtonText.text = "Okay";

        }
        //  Searching the Q1

        else
        {
            locationQuestions[locationCount].SetActive(false);
            instructionPanel.SetActive(false);
            questionButton.SetActive(false);
        }



    }

    public void locationGeneralButton(int locationNum, string locationName, ref List<GameObject> locationQuestions, ref int locationCount, TextMeshProUGUI buttonText, ref AudioClip[] locationSound)
    {
        currentLocation = locationNum;
        locationText.text = locationName;
        mapPanel.SetActive(false);
        instructionPanel.SetActive(true);

        //  showing location intro
        if (locationCount == 0)
        {
            locationQuestions[locationCount].SetActive(true);

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.PlayOneShot(locationSound[locationCount], 0.5f);

            questionButtonText.text = "Next";
        }

        else
        {
            if (isImageDetected[locationNum + "-" + locationCount])
            {
                if (isImageDetected[locationNum + "-3"])
                    instructionPanel.SetActive(false);
                

                else
                {
                    locationCount++;
                    locationQuestions[locationCount].SetActive(true);

                    if (audioSource.isPlaying)
                        audioSource.Stop();

                    audioSource.PlayOneShot(locationSound[locationCount], 0.5f);

                    ableToDetect[locationNum + "-" + locationCount] = true;
                }
            }
            else
            {

                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.PlayOneShot(locationSound[locationCount], 0.5f);

                locationQuestions[locationCount].SetActive(true);
            }
        }

        questionButtonText.text = "Okay";
        questionButton.SetActive(true);
    }


    // Update is called once per frame
    private void Update()
    {
        if (usernameText.text == "")
            usernameText.text = "Ibn Battuta";

        //output.text = "YYY " + ibnBattuta.activeInHierarchy;

    }
}
