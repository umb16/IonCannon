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
            case ItemType.SpeedDrives:
                return SpeedDrives();
            case ItemType.CoprocessorPlus:
                return CoprocessorPlus();
            case ItemType.AmplifierPlus:
                return AmplifierPlus();
            case ItemType.AmplifierPlusPlus:
                return AmplifierPlusPlus();
            case ItemType.LensSystemPlus:
                return LensSystemPlus();
            case ItemType.BatteryPlus:
                return BatteryPlus();
            case ItemType.BatteryPlusPlus:
                return BatteryPlusPlus();
            case ItemType.DeliveryDevicePlus:
                return DeliveryDevicePlus();
            case ItemType.DeliveryDevicePlusPlus:
                return DeliveryDevicePlusPlus();
            case ItemType.CoprocessorPlusPlus:
                return CoprocessorPlusPlus();
            case ItemType.IonizationUnitPlus:
                return IonizationUnitPlus();
            case ItemType.IonizationUnitPlusPlus:
                return IonizationUnitPlusPlus();
            case ItemType.PowerControllerPlus:
                return PowerControllerPlus();
            case ItemType.PowerControllerPlusPlus:
                return PowerControllerPlusPlus();
            case ItemType.ShiftSystem:
                return ShiftSystem();
            case ItemType.ReverseSystem:
                return ReverseSystem();
            case ItemType.None:
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
            Icon = Addresses.Ico_Battery,
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
    public Item BatteryPlus()
    {
        return new Item()
        {
            Type = ItemType.BatteryPlus,
            Name = "�������+",
            UpgradeCount = 1,
            Description = "������ ���� +75%",
            Cost = 75,
            Icon = Addresses.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.75f, StatModificatorType.Multiplicative, StatType.RayPathLenght)
                },
                    PerkType.RayPathLenght)
            },
        };
    }
    public Item BatteryPlusPlus()
    {
        return new Item()
        {
            Type = ItemType.BatteryPlusPlus,
            Name = "�������++",
            UpgradeCount = 2,
            Description = "������ ���� +150%",
            Cost = 75,
            Icon = Addresses.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.RayPathLenght)
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
            Icon = Addresses.Ico_Servo,
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
            Icon = Addresses.Ico_SpeedBoost,
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
            Icon = Addresses.Ico_Amplifier,
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
    public Item AmplifierPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.AmplifierPlus,
            Name = "���������+",
            Description = "������ ���� -30%\n���� ���� +40%",
            Cost = 150,
            Icon = Addresses.Ico_Amplifier,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.3f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(0.4f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.Amplifier)
            }
        };
    }
    public Item AmplifierPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.AmplifierPlusPlus,
            Name = "���������+",
            Description = "������ ���� -40%\n���� ���� +100%",
            Cost = 400,
            Icon = Addresses.Ico_Amplifier,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.4f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamage)
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
            Icon = Addresses.Ico_Lens,
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
            Description = "��������� ����� ����� �������� 4 ����� � ������� � ������� 10 ������",
            Cost = 60,
            Icon = Addresses.Ico_Radiation,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(4f, 10f)//����, ������������
            }
        };
    }
    public Item IonizationUnitPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.IonizationUnitPlus,
            Name = "���� ���������+",
            Description = "��������� ����� ����� �������� 6 ����� � ������� � ������� 15 ������",
            Cost = 180,
            Icon = Addresses.Ico_Radiation,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(6f, 15f)//����, ������������
            }
        };
    }
    public Item IonizationUnitPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.IonizationUnitPlusPlus,
            Name = "���� ���������++",
            Description = "��������� ����� ����� �������� 12 ����� � ������� � ������� 20 ������",
            Cost = 480,
            Icon = Addresses.Ico_Radiation,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(12f, 20f)//����, ������������
            }
        };
    }
    public Item ShiftSystem()
    {
        return new Item()
        {
            Unique = true,
            Type = ItemType.ShiftSystem,
            Name = "������� ������",
            Description = "��� ��������� ����� ������ �������������� �� 2 �������.\n����� �������� ���� ������������� �� 1.",
            Cost = 100,
            Icon = Addresses.Ico_ShiftSystem,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPShiftSystem>(),
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
            Icon = Addresses.Ico_Box,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(20f),//cooldown
            }
        };
    }
    public Item DeliveryDevicePlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.DeliveryDevicePlus,
            Name = "���������� ��������+",
            Description = "��� � 15 ������ ���������� � ������ ���� �� �����������",
            Cost = 150,
            Icon = Addresses.Ico_Box,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(15f),//cooldown
            }
        };
    }
    public Item DeliveryDevicePlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.DeliveryDevicePlusPlus,
            Name = "���������� ��������++",
            Description = "��� � 10 ������ ���������� � ������ ���� �� �����������",
            Cost = 400,
            Icon = Addresses.Ico_Box,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(10f),//cooldown
            }
        };
    }
    public Item Coprocessor()
    {
        return new Item()
        {
            Type = ItemType.Coprocessor,
            Name = "�����������",
            Description = "����� ��������� -30%\n����������� -1 �",
            Cost = 50,
            Icon = Addresses.Ico_Chip,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-.3f, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-1f, StatModificatorType.Additive, StatType.RayError),
                },
                    PerkType.RayDelay)
            },
        };
    }
    public Item CoprocessorPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.CoprocessorPlus,
            Name = "�����������+",
            Description = "����� ��������� -50%\n����������� -2 �",
            Cost = 150,
            Icon = Addresses.Ico_Chip,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-2f, StatModificatorType.Additive, StatType.RayError),
                },
                    PerkType.RayDelay)
            },
        };
    }

    public Item CoprocessorPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.CoprocessorPlusPlus,
            Name = "�����������++",
            Description = "����� ��������� -100%\n����������� -4 �",
            Cost = 400,
            Icon = Addresses.Ico_Chip,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-1, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-4f, StatModificatorType.Additive, StatType.RayError),
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
            Icon = Addresses.Ico_DivergingLens,
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
            Description = "���� +20%\n������ ���� +20%",
            Cost = 100,
            Icon = Addresses.Ico_PowerController,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                    PerkType.PowerController)
            },
        };
    }
    public Item PowerControllerPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.PowerControllerPlus,
            NotForSale = true,
            Name = "���������� �������+",
            Description = "���� +30%\n������ ���� +30%",
            Cost = 200,
            Icon = Addresses.Ico_PowerController,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                    PerkType.PowerController)
            },
        };
    }
    public Item PowerControllerPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.PowerControllerPlusPlus,
            NotForSale = true,
            Name = "���������� �������++",
            Description = "���� +60%\n������ ���� +60%",
            Cost = 500,
            Icon = Addresses.Ico_PowerController,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.RayDamage)
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
            Icon = Addresses.Ico_Lenses,
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
    public Item LensSystemPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.LensSystemPlus,
            NotForSale = true,
            Unique = false,
            Name = "������� ����+",
            Description = "������ ���� +200%",
            Cost = 400,
            Icon = Addresses.Ico_Lenses,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)
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
            Icon = Addresses.Ico_Magnet,
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
    public Item ReverseSystem()
    {
        return new Item()
        {
            Type = ItemType.ReverseSystem,
            Name = "������� ��������� ����.",
            Description = "�������� ���� +100%\n��� �������� ���� � �������.",
            Cost = 300,
            Icon = Addresses.Ico_Reverse,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Additive, StatType.RayReverse),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed)
                },
                PerkType.RayReverse)
            }
        };
    }
    public Item SpeedDrives()
    {
        return new Item()
        {
            Type = ItemType.SpeedDrives,
            Name = "���������� �������",
            Description = "�������� ���� +100%\n����������� +3 �",
            Cost = 100,
            Icon = Addresses.Ico_SpeedServo,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed),
                    new StatModificator(3, StatModificatorType.Additive, StatType.RayError),
                },
                PerkType.RaySpeed)
            }
        };
    }
}
