using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class mainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Text text;
    public void startGame()
    {
        //SceneManager.LoadScene(1);
        Sequence se = DOTween.Sequence();
        cg.gameObject.SetActive(true);
        se.Append(cg.DOFade(1, .5f));
        se.Append(text.DOText("       你是小主人玩具箱里面的一块橡皮泥，很受主人的喜欢。但有一天主人带回了一个超大的机甲玩具，他凭借着坚硬和庞大的身体排挤你生存的空间。你决定开始反抗……",1.5f));
        se.AppendCallback(() =>
        {
            SceneManager.LoadScene(1);
        });
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
