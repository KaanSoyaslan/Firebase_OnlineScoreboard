using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class FirebaseScoreBoard : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public DatabaseReference DBreference;


    public Sprite[] CarSprites;

    [Header("Scoreboard")] //ilk 20
    //scoreboard
    public Image[] sbCar;
    public TMP_Text[] sbName;
    public TMP_Text[] sbScore;

    [Header("EndSc")] //en iyi 3
    //endsc
    public Image[] esCar;
    public TMP_Text[] esName;
    public TMP_Text[] esScore;



    //E easy   M medium   H hard i�in scorelar
    public int[] ScoresE;
    public int[] CarE;
    public string[] NameE;

    public int[] ScoresM;
    public int[] CarM;
    public string[] NameM;

    public int[] ScoresH;
    public int[] CarH;
    public string[] NameH;


    //bizimkiler
    public int Kupa;
    public int Ara�;
    public string isim;

    public GameObject ScoreBoardSc;


    //en sonuncu ki�iler
    public static int LastScoreE;
    public static int LastScoreM;
    public static int LastScoreH;
    public static int FirstScoreH; //database y�klenmi�mi kontrol i�in ilk ki�i


    public TMP_InputField LiderKay�tNameIF;
    public TMP_Text LiderKay�tScoreTXT;

    public static bool Tetic = false;

    public TMP_Text HataTXT;
    public GameObject LeaderSaveSC;

    void Start()
    {

    }
    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
                Debug.Log("Ba�land�");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                // SoundManager.PlaySound("clickf");
            }
        });
    }
    private void InitializeFirebase()
    {
        //Set the authentication instance object
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;


    }
    // Update is called once per frame
    void Update()
    {
        if (Tetic)
        {
            Tetic = false;
            GetALLsCORES();
        }
    }
    public void buttoncode()
    {

        GetALLsCORES();
    }

    public void GetALLsCORES()
    {
        StartCoroutine(GetScoreHard());
        StartCoroutine(GetScoreEasy());
        StartCoroutine(GetScoreMedium());


    }
    public void ScoreRefresh()
    {
        if (PlayerPrefs.GetInt("GameTypeLevel") == 0 && PlayerPrefs.GetInt("GameType") == 0) //kolaysa sonsuzsa
        {
            ButtonEasy();
        }
        else if (PlayerPrefs.GetInt("GameTypeLevel") == 1 && PlayerPrefs.GetInt("GameType") == 0) //med sonsuzsa
        {
            ButtonMedium();
        }
        else if (PlayerPrefs.GetInt("GameTypeLevel") == 2 && PlayerPrefs.GetInt("GameType") == 0)//hard sonsuzsa
        {
            ButtonHard();
        }
        else
        {
            ButtonHard();
        }
    }
    private IEnumerator GetScoreHard()
    {
        var DBTask = DBreference.Child("ScoreH").OrderByChild("score").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            int i = 0;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {

                if (i < 20)
                {
                    ScoresH[i] = int.Parse(childSnapshot.Child("score").Value.ToString());
                    CarH[i] = int.Parse(childSnapshot.Child("car").Value.ToString());
                    NameH[i] = childSnapshot.Key;
                }
                i++;

            }
            ScoreRefresh();
        }
    }
    private IEnumerator GetScoreEasy()
    {
        var DBTask = DBreference.Child("ScoreE").OrderByChild("score").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            int i = 0;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (i < 20)
                {
                    ScoresE[i] = int.Parse(childSnapshot.Child("score").Value.ToString());
                    CarE[i] = int.Parse(childSnapshot.Child("car").Value.ToString());
                    NameE[i] = childSnapshot.Key;
                }
                i++;

            }

            ScoreRefresh();
        }
    }
    private IEnumerator GetScoreMedium()
    {
        var DBTask = DBreference.Child("ScoreM").OrderByChild("score").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            int i = 0;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {


                if (i < 20)
                {
                    ScoresM[i] = int.Parse(childSnapshot.Child("score").Value.ToString());
                    CarM[i] = int.Parse(childSnapshot.Child("car").Value.ToString());
                    NameM[i] = childSnapshot.Key;
                }
                i++;

            }
            ScoreRefresh();

        }
    }
    private IEnumerator SetScore(string ad, int num)
    {
        //Set the currently logged in user Coin
        var DBTask = DBreference.Child("ScoreH").Child(ad).Child("score").SetValueAsync(num);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {

        }
    }
    private IEnumerator SetCar(string ad, int num)
    {
        //Set the currently logged in user Coin
        var DBTask = DBreference.Child("ScoreH").Child(ad).Child("car").SetValueAsync(num);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {

        }
    }
    private IEnumerator SetScoreE(string ad, int num)
    {
        //Set the currently logged in user Coin
        var DBTask = DBreference.Child("ScoreE").Child(ad).Child("score").SetValueAsync(num);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {

        }
    }
    private IEnumerator SetCarE(string ad, int num)
    {
        //Set the currently logged in user Coin
        var DBTask = DBreference.Child("ScoreE").Child(ad).Child("car").SetValueAsync(num);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {

        }
    }
    private IEnumerator SetScoreM(string ad, int num)
    {
        //Set the currently logged in user Coin
        var DBTask = DBreference.Child("ScoreM").Child(ad).Child("score").SetValueAsync(num);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {

        }
    }
    private IEnumerator SetCarM(string ad, int num)
    {
        //Set the currently logged in user Coin
        var DBTask = DBreference.Child("ScoreM").Child(ad).Child("car").SetValueAsync(num);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {

        }
    }


    public void ButtonEasy() //Kolaydaki herkes
    {
        for (int i = 0; i < sbCar.Length; i++)
        {
            sbCar[i].sprite = CarSprites[CarE[i]];
            sbName[i].text = NameE[i];
            sbScore[i].text = ScoresE[i].ToString();
        }
        for (int i = 0; i < esCar.Length; i++)
        {
            esCar[i].sprite = CarSprites[CarE[i]];
            esName[i].text = NameE[i];
            esScore[i].text = ScoresE[i].ToString();
        }
        LastScoreE = int.Parse(ScoresE[19].ToString());
        FirstScoreH = int.Parse(ScoresH[0].ToString());
    }
    public void ButtonMedium()//meddeki herkes
    {
        for (int i = 0; i < sbCar.Length; i++)
        {
            sbCar[i].sprite = CarSprites[CarM[i]];
            sbName[i].text = NameM[i];
            sbScore[i].text = ScoresM[i].ToString();
        }
        for (int i = 0; i < esCar.Length; i++)
        {
            esCar[i].sprite = CarSprites[CarM[i]];
            esName[i].text = NameM[i];
            esScore[i].text = ScoresM[i].ToString();
        }
        LastScoreM = int.Parse(ScoresM[19].ToString());
        FirstScoreH = int.Parse(ScoresH[0].ToString());
    }
    public void ButtonHard()//zordaki herkes
    {
        for (int i = 0; i < sbCar.Length; i++)
        {
            sbCar[i].sprite = CarSprites[CarH[i]];
            sbName[i].text = NameH[i];
            sbScore[i].text = ScoresH[i].ToString();
        }
        for (int i = 0; i < esCar.Length; i++)
        {
            esCar[i].sprite = CarSprites[CarH[i]];
            esName[i].text = NameH[i];
            esScore[i].text = ScoresH[i].ToString();
        }
        LastScoreH = int.Parse(ScoresH[19].ToString());
        FirstScoreH = int.Parse(ScoresH[0].ToString());
    }
    public void ScoreBoardAcKapa(bool onOff)
    {
        ScoreBoardSc.SetActive(onOff);
    }
    public void SaveScore()
    {
        HataTXT.text = "";
        if (PlayerPrefs.GetInt("GameTypeLevel") == 2) //HARD
        {

            //ayn� isimli ba�kas� varm� kontrol
            bool Kontrol = false;
            for (int i = 0; i < NameH.Length; i++)
            {
                if (LiderKay�tNameIF.text == NameH[i])
                {
                    Kontrol = true;
                }
            }
            if (Kontrol)
            {
                if (PlayerPrefs.GetInt("Dil") == 1)
                {
                    HataTXT.text = "Bu isim daha �nceden kullan�lm��.";
                }
                else
                {
                    HataTXT.text = "Username already taken.";
                }
                LiderKay�tNameIF.text += "*";
            }
            else //yoksa KAYDET
            {
                StartCoroutine(SetScore(LiderKay�tNameIF.text, int.Parse(LiderKay�tScoreTXT.text)));
                StartCoroutine(SetCar(LiderKay�tNameIF.text, PlayerPrefs.GetInt("ChoosedCar")));


                LeaderSaveSC.SetActive(false);

            }



            // (1)
        }
        else if (PlayerPrefs.GetInt("GameTypeLevel") == 1)  //MED
        {

            bool Kontrol = false;
            for (int i = 0; i < NameM.Length; i++)
            {
                if (LiderKay�tNameIF.text == NameM[i])
                {
                    Kontrol = true;
                }
            }
            if (Kontrol)
            {
                if (PlayerPrefs.GetInt("Dil") == 1)
                {
                    HataTXT.text = "Bu isim daha �nceden kullan�lm��.";
                }
                else
                {
                    HataTXT.text = "Username already taken.";
                }
                LiderKay�tNameIF.text += "*";
            }
            else
            {
                StartCoroutine(SetScoreM(LiderKay�tNameIF.text, int.Parse(LiderKay�tScoreTXT.text)));
                StartCoroutine(SetCarM(LiderKay�tNameIF.text, PlayerPrefs.GetInt("ChoosedCar")));
                LeaderSaveSC.SetActive(false);


            }
        }
        else if (PlayerPrefs.GetInt("GameTypeLevel") == 0) //EASY
        {

            bool Kontrol = false;
            for (int i = 0; i < NameE.Length; i++)
            {
                if (LiderKay�tNameIF.text == NameE[i])
                {
                    Kontrol = true;
                }
            }
            if (Kontrol)
            {
                if (PlayerPrefs.GetInt("Dil") == 1)
                {
                    HataTXT.text = "Bu isim daha �nceden kullan�lm��.";
                }
                else
                {
                    HataTXT.text = "Username already taken.";
                }
                LiderKay�tNameIF.text += "*";
            }
            else
            {
                StartCoroutine(SetScoreE(LiderKay�tNameIF.text, int.Parse(LiderKay�tScoreTXT.text)));
                StartCoroutine(SetCarE(LiderKay�tNameIF.text, PlayerPrefs.GetInt("ChoosedCar")));

                LeaderSaveSC.SetActive(false);


            }
        }

        GetALLsCORES(); //Database e yollad�ktan sonra verileri yenile
    }


}

