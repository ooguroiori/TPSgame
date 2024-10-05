using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusWindowManager : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO;
    [SerializeField] TextMeshProUGUI hpValue;
    [SerializeField] TextMeshProUGUI mpValue;
    // Start is called before the first frame update
    void Start()
    {
        // hpValue.GetComponent<TextMeshProUGUI>().text = playerStatusSO.HP.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        hpValue.GetComponent<TextMeshProUGUI>().text = GameObject.Find("PlayerArmature").GetComponent<PlayerStatus>().HP.ToString();
        mpValue.GetComponent<TextMeshProUGUI>().text = GameObject.Find("PlayerArmature").GetComponent<PlayerStatus>().MP.ToString();
    }
}
