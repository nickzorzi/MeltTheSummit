using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour
{
    public GameObject energyPrefab;
    public PlayerController playerController;
    List<Energy> energies = new List<Energy>();

    private void OnEnable()
    {
        PlayerController.OnPlayerAbility += DrawEnergy;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerAbility -= DrawEnergy;
    }

    private void Start()
    {
        DrawEnergy();
    }

    public void DrawEnergy()
    {
        ClearEnergy();

        int energyToMake = (int)(playerController.abilityMaxCooldown);
        for (int i = 0; i < energyToMake; i++)
        {
            CreateEmptyEnergy();
        }

        for (int i = 0; i < energies.Count; i++)
        {
            int energyStatusRemainder = (int)Mathf.Clamp(playerController.abilityCooldown - (i * 1), 0, 1);
            energies[i].SetEnergyImage((EnergyStatus)energyStatusRemainder);
        }
    }

    public void CreateEmptyEnergy()
    {
        GameObject newEnergy = Instantiate(energyPrefab);
        newEnergy.transform.SetParent(transform);

        int insertIndex = energies.Count + 1;
        newEnergy.transform.SetSiblingIndex(insertIndex);

        Energy energyComponent = newEnergy.GetComponent<Energy>();
        energyComponent.SetEnergyImage(EnergyStatus.Empty);
        energies.Add(energyComponent);
    }

    public void ClearEnergy()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        energies = new List<Energy>();
    }
}
