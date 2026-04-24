using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject buyMessage;

    private bool inBuyZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int itemCost;

    // На сколько единиц поднимается максимальное здоровье при покупке апгрейда HP.
    public int healthUpgradeAmount = 1;

    void Start()
    {

    }

    void Update()
    {
        if (!inBuyZone) return;

        if (!Input.GetKeyDown(KeyCode.E)) return;

        TryPurchase();
    }

    private void TryPurchase()
    {
        // Защита от пустых ссылок, чтобы покупка не валилась с NullReference,
        // если на сцене забыли поставить Managers.
        if (LevelManager.instance == null)
        {
            Debug.LogWarning("[ShopItem] LevelManager.instance is null — на сцене нет LevelManager.", this);
            return;
        }

        if (LevelManager.instance.currentCoins < itemCost)
        {
            Debug.Log($"[ShopItem] Недостаточно монет: есть {LevelManager.instance.currentCoins}, нужно {itemCost}.", this);
            return;
        }

        // Сначала проверяем, что покупка вообще возможна / имеет смысл,
        // и только потом списываем монеты. Так у игрока не будут списываться
        // деньги впустую (например, при полном HP).
        bool purchased = false;

        if (isHealthRestore)
        {
            if (PlayerHealthController.instance == null)
            {
                Debug.LogWarning("[ShopItem] PlayerHealthController.instance is null — нет игрока на сцене.", this);
                return;
            }

            if (PlayerHealthController.instance.currentHealth >= PlayerHealthController.instance.maxHealth)
            {
                Debug.Log("[ShopItem] Здоровье уже полное — покупка лечения не выполнена.", this);
                return;
            }

            PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
            purchased = true;
        }
        else if (isHealthUpgrade)
        {
            if (PlayerHealthController.instance == null)
            {
                Debug.LogWarning("[ShopItem] PlayerHealthController.instance is null — нет игрока на сцене.", this);
                return;
            }

            PlayerHealthController.instance.maxHealth += healthUpgradeAmount;
            PlayerHealthController.instance.HealPlayer(healthUpgradeAmount);
            purchased = true;
        }
        else if (isWeapon)
        {
            // Оружие пока не реализовано — выдаём предупреждение, но монеты НЕ списываем,
            // чтобы не было ощущения «купил, а ничего не получил».
            Debug.LogWarning("[ShopItem] isWeapon = true, но логика выдачи оружия ещё не реализована.", this);
            return;
        }
        else
        {
            Debug.LogWarning("[ShopItem] Не указано, что именно продаёт этот предмет (isHealthRestore / isHealthUpgrade / isWeapon).", this);
            return;
        }

        if (purchased)
        {
            LevelManager.instance.SpendCoins(itemCost);
            Debug.Log($"[ShopItem] Покупка совершена за {itemCost} монет. Осталось: {LevelManager.instance.currentCoins}.", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (buyMessage != null) buyMessage.SetActive(true);

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (buyMessage != null) buyMessage.SetActive(false);

            inBuyZone = false;
        }
    }
}
