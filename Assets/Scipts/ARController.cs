using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace myNameSpace
{
    public class ARController : MonoBehaviour
    {
        public GameObject ibnBattuta;

        //  Albalad (1), Diriyah (2), Alula (3)
        public int currentLocation = 0;
        public TextMeshProUGUI locationText;

        public TextMeshProUGUI usernameText;
        public TMP_InputField usernameField;


        public GameObject instructionPanel;
        public GameObject mapPanel;

        public GameObject startButton;
        public GameObject questionButton;
        public GameObject usernameInputField;

        public TextMeshProUGUI mainButtonText;

        public int panelTextCount = 0;
        public List<GameObject> mainPanelText;

        public int albaladQuesCount = 0;
        public List<GameObject> albaladQuestion;

        public int alulaQuesCount = 0;
        public List<GameObject> alulaQuestion;

        public int diriyahQuesCount = 0;
        public List<GameObject> diriyahQuestion;

        public TextMeshProUGUI scoreNum;
        public static int scoreCount;

        public void play()
        {
            //  Displaying the instruction panel
            if (panelTextCount < mainPanelText.Count)
            {
                if (panelTextCount > 0)
                    mainPanelText[panelTextCount - 1].SetActive(false);
                instructionPanel.SetActive(true);
                print("panelTextCount: " + panelTextCount);
                mainPanelText[panelTextCount].SetActive(true);

                if (panelTextCount == 1)
                    mainButtonText.text = "YES!";

                else if (panelTextCount == 4)
                {
                    usernameText.text = usernameField.text;

                    usernameInputField.SetActive(false);
                    mainButtonText.text = "Okay";
                }

                else
                {
                    mainButtonText.text = "Next";
                    if (panelTextCount == 3)
                    {
                        usernameInputField.SetActive(true);
                    }
                }

                panelTextCount++;

            }
            else
            {
                instructionPanel.SetActive(false);
                mainPanelText[panelTextCount - 1].SetActive(false);
                startButton.SetActive(false);
            }

        }


        // Start is called before the first frame update
        public void question()
        {
            //  Abalad Quesions
            if (currentLocation == 1)
            {
                instructionPanel.SetActive(false);
                albaladQuestion[albaladQuesCount].SetActive(false);
                questionButton.SetActive(false);
            }

            //  Alula Quesions
            else if (currentLocation == 2)
            {
                instructionPanel.SetActive(false);
                albaladQuestion[albaladQuesCount].SetActive(false);
                questionButton.SetActive(false);
            }

            //  Diriyah Quesions
            else if (currentLocation == 3)
            {
                instructionPanel.SetActive(false);
                albaladQuestion[albaladQuesCount].SetActive(false);
                questionButton.SetActive(false);
            }
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
            currentLocation = 1;
            locationText.text = "Albalad";
            mapPanel.SetActive(false);
            instructionPanel.SetActive(true);
            albaladQuestion[albaladQuesCount].SetActive(true);
            questionButton.SetActive(true);
        }

        public void alulaButton()
        {
            currentLocation = 2;
            locationText.text = "Alula";
            mapPanel.SetActive(false);
            instructionPanel.SetActive(true);
            alulaQuestion[alulaQuesCount].SetActive(true);
            questionButton.SetActive(true);
        }

        public void diriyahButton()
        {
            currentLocation = 3;
            locationText.text = "Diriyah";
            mapPanel.SetActive(false);
            instructionPanel.SetActive(true);
            diriyahQuestion[diriyahQuesCount].SetActive(true);
            questionButton.SetActive(true);
        }


        // Update is called once per frame
        private void Update()
        {
            if (usernameText.text == "")
                usernameText.text = "Ibn Battuta";

            //output.text = "YYY " + ibnBattuta.activeInHierarchy;

        }


        public void increaseScore(string name)
        {
            if (!ImagesMarkerSceneManager.isImageDetected[name])
            {
                scoreCount += 50;
                scoreNum.text = "" + scoreCount;
            }
            ImagesMarkerSceneManager.isImageDetected[name] = true;

        }
    }
}
//ibnBattuta.SetActive(false);