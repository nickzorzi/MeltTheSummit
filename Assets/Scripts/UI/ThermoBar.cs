using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermoBar : MonoBehaviour
{
    public GameObject thermoPrefab;
    public PlayerController playerController;
    List<Thermo> thermos = new List<Thermo>();

    private void OnEnable()
    {
        PlayerController.OnPlayerThermo += DrawThermos;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerThermo -= DrawThermos;
    }

    private void Start()
    {
        DrawThermos();
    }

    public void DrawThermos()
    {
        ClearThermos();

        float maxThermoRemainder = playerController.maxTemp % 2;
        int thermoToMake = (int)(playerController.maxTemp / 4 + maxThermoRemainder);
        for (int i = 0; i < thermoToMake; i++)
        {
            CreateEmptyThermo();
        }

        for (int i = 0; i < thermos.Count; i++)
        {
            int thermoStatusRemainder = (int)Mathf.Clamp(playerController.temp - (i * 4), 0, 4);
            thermos[i].SetThermoImage((ThermoStatus)thermoStatusRemainder);
        }
    }

    public void CreateEmptyThermo()
    {
        GameObject newThermo = Instantiate(thermoPrefab);
        newThermo.transform.SetParent(transform);

        int insertIndex = thermos.Count + 1;
        newThermo.transform.SetSiblingIndex(insertIndex);

        Thermo thermoComponent = newThermo.GetComponent<Thermo>();
        thermoComponent.SetThermoImage(ThermoStatus.Empty);
        thermos.Add(thermoComponent);
    }

    public void ClearThermos()
    {
        foreach (Transform t in transform)
        {
            if (t.name == "Front" || t.name == "Back")
            {
                continue;
            }

            Destroy(t.gameObject);
        }
        thermos = new List<Thermo>();
    }
}
