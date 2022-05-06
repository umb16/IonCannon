using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsDB
{
    private ItemType[] shop ={
        ItemType.AdditionalDrives,
        ItemType.ExoskeletonSpeedBooster,
         ItemType.Battery,
         ItemType.Amplifier,
         ItemType.FocusLens,
         ItemType.IonizationUnit,
         ItemType.DeliveryDevice,
         ItemType.Coprocessor,
         ItemType.DivergingLens,
         ItemType.MagneticManipulator,
    };
    private PerksFactory _perksFactory;

    private ItemsDB(PerksFactory perksFactory)
    {
        _perksFactory = perksFactory;
    }

    public Item GetRandomItem()
    {
        var randomEnum = shop[Random.Range(0, shop.Length)];
        return CreateByType(randomEnum);
    }
    public Item CreateByType(ItemType type)
    {
        switch (type)
        {
            case ItemType.AdditionalDrives:
                return AdditionalDrives();
            case ItemType.ExoskeletonSpeedBooster:
                return ExoskeletonSpeedBooster();
            case ItemType.Battery:
                return Battery();
            case ItemType.Amplifier:
                return Amplifier();
            case ItemType.FocusLens:
                return FocusLens();
            case ItemType.IonizationUnit:
                return IonizationUnit();
            case ItemType.DeliveryDevice:
                return DeliveryDevice();
            case ItemType.Coprocessor:
                return Coprocessor();
            case ItemType.DivergingLens:
                return DivergingLens();
            case ItemType.PowerController:
                return PowerController();
            case ItemType.LensSystem:
                return LensSystem();
            case ItemType.MagneticManipulator:
                return MagneticManipulator();
            default:
                return Battery();
        }
    }
    public Item Battery()
    {
        return new Item()
        {
            Type = ItemType.Battery,
            Name = "�������",
            Description = "������ ���� +50%",
            Cost = 30,
            Icon = AddressKeys.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RayPathLenght)
                },
                    PerkType.RayPathLenght)
            },
        };
    }
    public Item AdditionalDrives()
    {
        return new Item()
        {
            Type = ItemType.AdditionalDrives,
            Name = "�������������� �������",
            Description = "�������� ���� +50%",
            Cost = 50,
            Icon = AddressKeys.Ico_Servo,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RaySpeed)
                },
                PerkType.RaySpeed)
            }
        };
    }
    public Item ExoskeletonSpeedBooster()
    {
        return new Item()
        {
            Type = ItemType.ExoskeletonSpeedBooster,
            Name = "���������� �����������",
            Description = "�������� ���� +0.5 �/�",
            Cost = 60,
            Icon = AddressKeys.Ico_SpeedBoost,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(0.5f, StatModificatorType.Additive, StatType.MovementSpeed)
                },
                PerkType.Speed)
            }
        };
    }
    public Item Amplifier()
    {
        return new Item()
        {
            Type = ItemType.Amplifier,
            Name = "���������",
            Description = "������ ���� -20%\n���� ���� +20%",
            Cost = 50,
            Icon = AddressKeys.Ico_Amplifier,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.2f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(0.2f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.Amplifier)
            }
        };
    }

    public Item FocusLens()
    {
        return new Item()
        {
            Type = ItemType.FocusLens,
            Unique = true,
            Name = "������������ �����",
            Description = "���� ���� +50%\n������ ���� -90%",
            Cost = 70,
            Icon = AddressKeys.Ico_Lens,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.9f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.FocusLens)
            }
        };
    }
    public Item IonizationUnit()
    {
        return new Item()
        {
            Type = ItemType.IonizationUnit,
            Name = "���� ���������",
            Description = "��������� ����� ����� �������� ������������� ���� 10% � �������",
            Cost = 60,
            Icon = AddressKeys.Ico_Radiation,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>()
            }
        };
    }
    public Item DeliveryDevice()
    {
        return new Item()
        {
            Type = ItemType.DeliveryDevice,
            Name = "���������� ��������",
            Description = "��� � 20 ������ ���������� � ������ ���� �� �����������",
            Cost = 50,
            Icon = AddressKeys.Ico_Box,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(),
            }
        };
    }
    public Item Coprocessor()
    {
        return new Item()
        {
            Type = ItemType.Coprocessor,
            Name = "�����������",
            Description = "����� ��������� -30%",
            Cost = 50,
            Icon = AddressKeys.Ico_Chip,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-.3f, StatModificatorType.Multiplicative, StatType.RayDelay)
                },
                    PerkType.RayDelay)
            },
        };
    }
    public Item DivergingLens()
    {
        return new Item()
        {
            Type = ItemType.DivergingLens,
            Unique = false,
            Name = "������������ �����",
            Description = "���� ���� -50%\n������ ���� +200%",
            Cost = 60,
            Icon = AddressKeys.Ico_DivergingLens,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(-0.5f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.DivergingLens)
            }
        };
    }
    public Item PowerController()
    {
        return new Item()
        {
            Type = ItemType.PowerController,
            NotForSale = true,
            Name = "���������� �������",
            Description = "���� +10%\n������ ���� +20%",
            Cost = 100,
            Icon = AddressKeys.Ico_PowerController,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(.1f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                    PerkType.PowerController)
            },
        };
    }
    public Item LensSystem()
    {
        return new Item()
        {
            Type = ItemType.LensSystem,
            NotForSale = true,
            Unique = false,
            Name = "������� ����",
            Description = "������ ���� +100%",
            Cost = 150,
            Icon = AddressKeys.Ico_Lenses,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)
                },
                PerkType.LensSystem)
            }
        };
    }
    public Item MagneticManipulator()
    {
        return new Item()
        {
            Type = ItemType.MagneticManipulator,
            Unique = false,
            Name = "��������� �����������",
            Description = "������ ����� + 100%",
            Cost = 40,
            Icon = AddressKeys.Ico_Magnet,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.PickupRadius)
                },
                PerkType.MagneticManipulator)
            }
        };
    }
}
