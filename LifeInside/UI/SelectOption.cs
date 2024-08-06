using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SelectOption : MonoBehaviour
{
    Episode selectedEp;
    public void SetSubEvent(Episode episode)
    {
        GameManager.I.uIController.uIEffect.EffectStart();
        List<Episode> sortEpi;
        sortEpi = GameManager.I.eventManager.sortByLocation(episode.location);
        sortEpi = sortEpi.Where(item => item.level       == episode.level).ToList();
        sortEpi = sortEpi.Where(item => item.defaultEpi  == false         ).ToList();
        int selectedNo = Prob.calcProb(sortEpi);

        if( selectedNo == -1 ){ return; }

        selectedEp = sortEpi[selectedNo]; // out of range. non negative
    }
    public void OnClickAccept()
    {
        gameObject.SetActive(false);

        if(selectedEp != null){
            GameManager.I.eventManager.StartAddEvent(selectedEp);
        }
    }

    public void OnClickReject()
    {
        GameManager.I.uIController.uIEffect.EffectEnd();
        gameObject.SetActive(false);
    }

    
}
