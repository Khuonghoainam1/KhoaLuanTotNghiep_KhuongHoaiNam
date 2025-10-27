using Thanh.Core;
using TMPro;
using UnityEngine.UI;

public class PausePanelController : Popup
{
   public Button resumeBtn;
    public Button homeBtn;
    public Button reTryBtn;
    public TMP_Text txtStages;
    //pulic Button TochtoPlay;

    private void OnEnable()
    {
        if(GlobalData.gameMode == GameMode.Normal)
        {
            txtStages.text = "STAGE " + (GlobalData.instance.levelToPlay + 1).ToString();
        }
        else
        {
            txtStages.text = "STAGE";
        }
        resumeBtn.onClick.AddListener(

            // AudioAssistant.PlaySound("BtnClick");
            Resume
        );
        homeBtn.onClick.AddListener(

            // AudioAssistant.PlaySound("BtnClick");
            Home
        );
        reTryBtn.onClick.AddListener(Retry);

        //TochtoPlay.onClick.AddListener(Resume);
    }

    private void OnDisable()
    {
        
        resumeBtn.onClick.RemoveListener(Resume);
        homeBtn.onClick.RemoveListener(Home);
        reTryBtn.onClick.RemoveListener(Retry);
    }

    public void Resume()
    {
        GameManager.Instance.ResumeGame();
        this.Close();
    }
    public void Retry()
    {
        // base.Close();
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
    }

    public void Home()
    {
        
        GlobalData.gameMode = GameMode.Home;
        User.Instance.Save();
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
        this.Close();
    }
}
