using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int Money = 0;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 비용 치를 수 있는지?
    /// </summary>
    public bool CanBuy(int price)
    {
        if (Money < price)
        {
            return false;
        }

        Money -= price;

        return true;
    }
}