using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsDB
{
    private Dictionary<ItemId, ItemTemplate> _templates = new Dictionary<ItemId, ItemTemplate>();
    private PerksFactory _perksFactory;

    private ItemId[] _shopItems ={
         //ItemType.AdditionalDrives,
         //ItemType.ExoskeletonSpeedBooster,
         //ItemType.Battery,
         //ItemType.Amplifier,
         //ItemType.FocusLens,
         //ItemType.IonizationUnit,
         //ItemType.DeliveryDevice,
         //ItemType.Coprocessor,
         //ItemType.DivergingLens,
         //ItemType.MagneticManipulator,
         //ItemType.AtomicBattery
         ItemId.Accelerator,
         ItemId.ArmoredPlates,
         ItemId.ExoskeletonModule,
         ItemId.MagneticCore,
         ItemId.OxygenCylinders,
         ItemId.CoprocessorTwo,
         ItemId.FreonCylinders,
         ItemId.DeliveryDeviceTwo,
         ItemId.AtomicCell,
         ItemId.Accumulator,
         ItemId.AdditionalDriveUnits,
         ItemId.AdditionalLens,
    };

    public Item CreateItem(ItemId itemId)
    {
        if (_templates.TryGetValue(itemId, out var value))
            return value.CreateItem();
        return null;
    }
    public Item CreateRandomItem()
    {
        var randomEnum = _shopItems[Random.Range(0, _shopItems.Length)];
        return CreateItem(randomEnum);
    }

    private void AddItemTemplate(ItemTemplate itemTemplate)
    {
        if (_templates.ContainsKey(itemTemplate.Id))
        {
            Debug.LogError($"ItemTemplateID {itemTemplate.Id} duplicate");
        }
        _templates[itemTemplate.Id] = itemTemplate;
    }



    private ItemsDB(PerksFactory perksFactory)
    {
        _perksFactory = perksFactory;

        AddItemTemplate(
        new ItemTemplate(LocaleKeys.Main.i_Accelerator)//Акселератор
        {
            Id = ItemId.Accelerator,
            Cost = 40,
            IconAddress = Addresses.Ico_Accelerator,
            CreatePerksFunc = () => new IPerk[]
            {
                new SimplePerk(PerkType.Speed,
                               new StatModificator(.1f, StatModificatorType.Multiplicative, StatType.MovementSpeed))//+10% скорости
            }
        });

        AddItemTemplate(//Бронированные пластины
            new ItemTemplate(LocaleKeys.Main.i_ArmoredPlates)
            {
                Id = ItemId.ArmoredPlates,
                Cost = 40,
                IconAddress = Addresses.Ico_ArmoredPlates,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerMaxHP,
                               new StatModificator(3f, StatModificatorType.Additive, StatType.MaxHP))//+3 к макс хп.
                }
            });

        AddItemTemplate(//Модуль экзоскелета
             new ItemTemplate(LocaleKeys.Main.i_ExoskeletonModule)
             {
                 Id = ItemId.ExoskeletonModule,
                 Cost = 40,
                 IconAddress = Addresses.Ico_ExoskeletonModule,
                 CreatePerksFunc = () => new IPerk[]
                 {
                new SimplePerk(PerkType.Speed,
                               new StatModificator(.5f, StatModificatorType.Additive, StatType.MovementSpeed))//+.5 скорости
                 }
             });
        AddItemTemplate(//Магнитное ядро
            new ItemTemplate(LocaleKeys.Main.i_MagneticCore)
            {
                Id = ItemId.MagneticCore,
                Cost = 40,
                IconAddress = Addresses.Ico_MagneticCore,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.MagneticManipulator,
                               new StatModificator(1f, StatModificatorType.Multiplicative, StatType.PickupRadius))//+100% радиус подбора
                }
            });
        AddItemTemplate(//Кислородные баллоны
            new ItemTemplate(LocaleKeys.Main.i_OxygenCylinders)
            {
                Id = ItemId.OxygenCylinders,
                Cost = 40,
                IconAddress = Addresses.Ico_OxygenCylinders,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(3f, StatModificatorType.Additive, StatType.LifeSupport))//+3 c лайвсапорта
                }
            });
        AddItemTemplate(//Сопроцессор
            new ItemTemplate(LocaleKeys.Main.i_CoprocessorTwo)
            {
                Id = ItemId.CoprocessorTwo,
                Cost = 40,
                IconAddress = Addresses.Ico_CoprocessorTwo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerDodge,
                               new StatModificator(.15f, StatModificatorType.Additive, StatType.Dodge))//+15% а шкансу клониться от урона
                }
            });
        AddItemTemplate( //Баллоны хладона
            new ItemTemplate(LocaleKeys.Main.i_FreonCylinders)
            {
                Id = ItemId.FreonCylinders,
                Cost = 40,
                IconAddress = Addresses.Ico_FreonCylinders,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerFireResist,
                               new StatModificator(2f, StatModificatorType.Additive, StatType.FireAbsorption))//+2 защиты от огня
                }
            });
        AddItemTemplate( //устройство доставки  
            new ItemTemplate(LocaleKeys.Main.i_DeliveryDevice)
            {
                Id = ItemId.DeliveryDeviceTwo,
                Cost = 40,
                IconAddress = Addresses.Ico_DeliveryDevice,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPBarrels>(24f),//раз в 24 с. сбрасывает ящик со взрывчаткой.
                }
            });
        AddItemTemplate( //Атомная ячейка
            new ItemTemplate(LocaleKeys.Main.i_AtomicCell)
            {
                Id = ItemId.AtomicCell,
                Cost = 40,
                IconAddress = Addresses.Ico_AtomicCell,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(2, StatModificatorType.Additive, StatType.EnergyRegen))//генерация энегрии +2 ед.
                }
            });
        AddItemTemplate( //Аккумулятор
            new ItemTemplate(LocaleKeys.Main.i_Accumulator)
            {
                Id = ItemId.Accumulator,
                Cost = 40,
                IconAddress = Addresses.Ico_Accumulator,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(.4f, StatModificatorType.Multiplicative, StatType.Capacity))//базовая емкость энергии +40%.
                }
            });
        AddItemTemplate( //Дополнительные приводы
            new ItemTemplate(LocaleKeys.Main.i_AdditionalDriveUnits)
            {
                Id = ItemId.AdditionalDriveUnits,
                Cost = 40,
                IconAddress = Addresses.Ico_AdditionalDriveUnits,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RaySpeed))//базовая скорость луча +30%.
                }
            });
        AddItemTemplate( //Дополнительная линза
            new ItemTemplate(LocaleKeys.Main.i_AdditionalLens)
            {
                Id = ItemId.AdditionalLens,
                Cost = 40,
                IconAddress = Addresses.Ico_AdditionalLens,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayDamage))//Базовый урон луча +20%.
                }
            });
        /////////////////////////////////////////////////////////////////////////////////////////желтые арты
        AddItemTemplate( //Дополнительный контур брони
            new ItemTemplate(LocaleKeys.Main.i_AdditionalArmorContour)
            {
                Id = ItemId.AdditionalArmorContour,
                Cost = 80,
                IconAddress = Addresses.Ico_AdditionalArmorContour,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(5f, StatModificatorType.Additive, StatType.MaxHP))//+5 к макс хп
                }
            });
        AddItemTemplate( //Ребра жесткости
            new ItemTemplate(LocaleKeys.Main.i_StiffeningRibs)
            {
                Id = ItemId.StiffeningRibs,
                Cost = 80,
                IconAddress = Addresses.Ico_StiffeningRibs,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerStunResist,
                    new StatModificator(.5f, StatModificatorType.Additive, StatType.StunResist))//+50% защиты от оглушения
                }
            });
        AddItemTemplate( //Подвижные механизмы
            new ItemTemplate(LocaleKeys.Main.i_MovableMechanisms)
            {
                Id = ItemId.MovableMechanisms,
                Cost = 80,
                IconAddress = Addresses.Ico_MovableMechanisms,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.Speed,
                    new StatModificator(.8f, StatModificatorType.Additive, StatType.MovementSpeed))//+.8 скорости
                }
            });
        AddItemTemplate( //Усиленное магнитное ядро
            new ItemTemplate(LocaleKeys.Main.i_ReinforcedMagneticCore)
            {
                Id = ItemId.ReinforcedMagneticCore,
                Cost = 80,
                IconAddress = Addresses.Ico_ReinforcedMagneticCore,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.PickupRadius))//+150% радиус подбора
                }
            });
        AddItemTemplate( //Кислородный распылитель
            new ItemTemplate(LocaleKeys.Main.i_OxygenSprayer)
            {
                Id = ItemId.OxygenSprayer,
                Cost = 80,
                IconAddress = Addresses.Ico_OxygenSprayer,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerSlowdownResist,
                    new StatModificator(.5f, StatModificatorType.Additive, StatType.SlowdownResist))//+50% защиты от замедления
                }
            });
        AddItemTemplate( //Анализирующий модуль
            new ItemTemplate(LocaleKeys.Main.i_AnalyzingModule)
            {
                Id = ItemId.AnalyzingModule,
                Cost = 80,
                IconAddress = Addresses.Ico_AnalyzingModule,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerDodge,
                    new StatModificator(.2f, StatModificatorType.Additive, StatType.Dodge))//+20% а шкансу клониться от урона
                }
            });
        AddItemTemplate( //Распылитель хладона
            new ItemTemplate(LocaleKeys.Main.i_FreonSprayer)
            {
                Id = ItemId.FreonSprayer,
                Cost = 80,
                IconAddress = Addresses.Ico_FreonSprayer,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(.3f, StatModificatorType.Additive, StatType.FireResist)),//+30% защиты от огня
                new SimplePerk(PerkType.PlayerElectricityResist,
                    new StatModificator(.3f, StatModificatorType.Additive, StatType.ElectricityResist))//+30% уворота от электричества
                }
            });
        AddItemTemplate( //Модуль подачи кислорода
            new ItemTemplate(LocaleKeys.Main.i_OxygenSupplyModule)
            {
                Id = ItemId.OxygenSupplyModule,
                Cost = 80,
                IconAddress = Addresses.Ico_OxygenSupplyModule,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerLifeSupport,
                    new StatModificator(4f, StatModificatorType.Additive, StatType.LifeSupport))//+4 c лайвсапорта
                }
            });
        AddItemTemplate( //Балоны отравляющего газа
            new ItemTemplate(LocaleKeys.Main.i_PoisonGasCylinders)
            {
                Id = ItemId.PoisonGasCylinders,
                Cost = 80,
                IconAddress = Addresses.Ico_PoisonGasCylinders,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Оставляет за собой дымку [[Ядовитый газ]].
                }
            });
        AddItemTemplate( //Сброс шлаков переработки
            new ItemTemplate(LocaleKeys.Main.i_ProcessingSlagDropping)
            {
                Id = ItemId.ProcessingSlagDropping,
                Cost = 80,
                IconAddress = Addresses.Ico_ProcessingSlagDropping,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Раз в 16 с. в случайную точку [[Зона контроля]] падает снаряд, наносящий 60 ед.
                    //[[Физический урон]] в центре, и 20 ед. по бокам.
                    //Радиус поражения - 10 п.
                }
            });
        AddItemTemplate( //Скоростные приводы
            new ItemTemplate(LocaleKeys.Main.i_HighSpeedDriveUnits)
            {
                Id = ItemId.HighSpeedDriveUnits,
                Cost = 80,
                IconAddress = Addresses.Ico_HighSpeedDriveUnits,
                CreatePerksFunc = () => new IPerk[]
                {
               new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RayError)),//[[Базовая скорость луча]] +50 %.                             
               new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//Погрешность + 1п.)
                }
            });
        AddItemTemplate( //Доставка взрывоопасных веществ
            new ItemTemplate(LocaleKeys.Main.i_DeliveryOfExplosives)
            {
                Id = ItemId.DeliveryOfExplosives,
                Cost = 80,
                IconAddress = Addresses.Ico_DeliveryOfExplosives,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPBarrels>(16f),//cooldown//Раз в 16 с. сбрасывает [[Ящик со взрывчаткой]].
                }
            });
        AddItemTemplate( //Ионизирующая установка
            new ItemTemplate(LocaleKeys.Main.i_IonizingPlant)
            {
                Id = ItemId.IonizingPlant,
                Cost = 80,
                IconAddress = Addresses.Ico_IonizingPlant,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPIonization>(4f, 4f)//Пораженные лучем враги получают статус [[Радиация]] с уроном в 4 единицы.
                }
            });
        AddItemTemplate( //Стабильная атомная ячейка
            new ItemTemplate(LocaleKeys.Main.i_StableAtomicCell)
            {
                Id = ItemId.StableAtomicCell,
                Cost = 80,
                IconAddress = Addresses.Ico_StableAtomicCell,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(3, StatModificatorType.Additive, StatType.EnergyRegen))//[[Генерация энергии]] +3 ед./с.
                }
            });
        AddItemTemplate( //Аккумуляторная батарея
            new ItemTemplate(LocaleKeys.Main.i_AccumulatorBattery)
            {
                Id = ItemId.AccumulatorBattery,
                Cost = 80,
                IconAddress = Addresses.Ico_AccumulatorBattery,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.Capacity))//[[Базовая ёмкость энергии]] + 60%.
                }
            });
        AddItemTemplate( //Контроллер питания
            new ItemTemplate(LocaleKeys.Main.i_PowerControllerTwo)
            {
                Id = ItemId.PowerControllerTwo,
                Cost = 80,
                IconAddress = Addresses.Ico_PowerControllerTwo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayCostReduction,
                               new StatModificator(1, StatModificatorType.Additive, StatType.RayCostReduction))//Уменьшает стоимсоть луча за пройденную длину луча.

                }
            });
        AddItemTemplate( //рассеивающий объектив
            new ItemTemplate(LocaleKeys.Main.i_DiffusionObjective)
            {
                Id = ItemId.DiffusionObjective,
                Cost = 80,
                IconAddress = Addresses.Ico_DiffusionObjective,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(-.4f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] -40 %.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] +80 %
                }
            });
        AddItemTemplate( //Вычислитель погрешностей
            new ItemTemplate(LocaleKeys.Main.i_ErrorCalculator)
            {
                Id = ItemId.ErrorCalculator,
                Cost = 80,
                IconAddress = Addresses.Ico_ErrorCalculator,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayError,
                              new StatModificator(-3, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] уменьшается на -3.
                }
            });
        AddItemTemplate( //Фокусирующий объектив
            new ItemTemplate(LocaleKeys.Main.i_FocusingObjective)
            {
                Id = ItemId.FocusingObjective,
                Cost = 80,
                IconAddress = Addresses.Ico_FocusingObjective,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.4f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +40 %.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(-0.4f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)),//[[Радиус поражения луча]] -40 %.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]]Погрешность + 1п.
                }
            });
        AddItemTemplate( //Нагнетатель плазмы
            new ItemTemplate(LocaleKeys.Main.i_PlasmaSupercharger)
            {
                Id = ItemId.PlasmaSupercharger,
                Cost = 80,
                IconAddress = Addresses.Ico_PlasmaSupercharger,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Накапливает заряд, следующий луч оставляет огненный след.
                }
            });

        //////////////////////////////////////////////////////////////////////////////////////////////оранживые арты
        AddItemTemplate( //Зеркало искажения
            new ItemTemplate(LocaleKeys.Main.i_DistortionMirror)
            {
                Id = ItemId.DistortionMirror,
                Cost = 160,
                IconAddress = Addresses.Ico_DistortionMirror,
                CreatePerksFunc = () => new IPerk[]
                {
                    //_perksFactory.Create<PerkPShiftSystem>(),//После получаения урона персонаж получает статус [[Имматериальный]] на 3 с.
                    //Наносимый персонажу урон увеличивается на 1.
                }
            });
        AddItemTemplate( //Фиолетовый покров
            new ItemTemplate(LocaleKeys.Main.i_PurpleCovering)
            {
                Id = ItemId.PurpleCovering,
                Cost = 160,
                IconAddress = Addresses.Ico_PurpleCovering,
                CreatePerksFunc = () => new IPerk[]
                {
                //При получении урона выпускает волну [[Ядовитый газ]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerMaxHP,
                               new StatModificator(5f, StatModificatorType.Additive, StatType.MaxHP))//[[Мак хит поинтов]] +5 ед.
                }
            });
        AddItemTemplate( //Защита экзоскелета
            new ItemTemplate(LocaleKeys.Main.i_ExoskeletonProtection)
            {
                Id = ItemId.ExoskeletonProtection,
                Cost = 160,
                IconAddress = Addresses.Ico_ExoskeletonProtection,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(10f, StatModificatorType.Additive, StatType.MaxHP))//[[Мак хит поинтов]] + 10 ед.
                }
            });
        AddItemTemplate( //Усиленный каркас
            new ItemTemplate(LocaleKeys.Main.i_ReinforcedCarcass)
            {
                Id = ItemId.ReinforcedCarcass,
                Cost = 160,
                IconAddress = Addresses.Ico_ReinforcedCarcass,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerStunResist,
                    new StatModificator(1f, StatModificatorType.Additive, StatType.StunResist))//[[Оглушение]] не действует на персонажа.
                }
            });
        AddItemTemplate( //Ускоритель экзоскелета
            new ItemTemplate(LocaleKeys.Main.i_ExoskeletonBooster)
            {
                Id = ItemId.ExoskeletonBooster,
                Cost = 160,
                IconAddress = Addresses.Ico_ExoskeletonBooster,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.Speed,
                               new StatModificator(1.6f, StatModificatorType.Additive, StatType.MovementSpeed))//[[Скорость передвижения]] +1.6 п/с.
                }
            });
        AddItemTemplate( //Магнитный манипулятор
            new ItemTemplate(LocaleKeys.Main.i_MagneticManipulatorTwo)
            {
                Id = ItemId.MagneticManipulatorTwo,
                Cost = 160,
                IconAddress = Addresses.Ico_MagneticManipulatorTwo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(3f, StatModificatorType.Multiplicative, StatType.PickupRadius))//[[Радиус сбора]] + 300%.
                }
            });
        AddItemTemplate( //Ядро электромагнитного поля
            new ItemTemplate(LocaleKeys.Main.i_ElectromagneticFieldCore)
            {
                Id = ItemId.ElectromagneticFieldCore,
                Cost = 160,
                IconAddress = Addresses.Ico_ElectromagneticFieldCore,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerRadiationResist,
                    new StatModificator(.9f, StatModificatorType.Additive, StatType.RadiationResist))//Уменьшает на 90% урон от [[Радиация]].
                }
            });
        AddItemTemplate( //Звездная пыль
            new ItemTemplate(LocaleKeys.Main.i_StarDust)
            {
                Id = ItemId.StarDust,
                Cost = 160,
                IconAddress = Addresses.Ico_StarDust,
                CreatePerksFunc = () => new IPerk[]
                {
                new PerkStarDust(10, 2, 5, 0, .5f, 10, 6, PerkType.GravityMatter),
                //При поднятии руды получаете малый осколок материи, который  вращается вокруг персонажа в радиусе 30 п., наносящий 5 ед.
                //[[Физический урон]] в течение 20 с.
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.PickupRadius))//[[Радиус сбора]] +150 %.
                }
            });
        AddItemTemplate( //Звездный осколок
            new ItemTemplate(LocaleKeys.Main.i_StarShard)
            {
                Id = ItemId.StarShard,
                Cost = 160,
                IconAddress = Addresses.Ico_StarShard,
                CreatePerksFunc = () => new IPerk[]
                {
                new PerkGravityMatter(20, 1.5f, 20, 1, 1, PerkType.GravityMatter)//Вращает осколок материи вокруг персонажа в радиусе 15 п.
                                                                                 //Осколок наносит 20 ед.  [[Физический урон]] и оглушает на 1 секунды.
                }
            });
        AddItemTemplate( //Система окисления
            new ItemTemplate(LocaleKeys.Main.i_OxidationSystem)
            {
                Id = ItemId.OxidationSystem,
                Cost = 160,
                IconAddress = Addresses.Ico_OxidationSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerSlowdownResist,
                    new StatModificator(1f, StatModificatorType.Additive, StatType.SlowdownResist))//Жижа не замедляет.
                                                                                                   //Жижа  растворяется.
                }
            });
        AddItemTemplate( //Пожарная система
            new ItemTemplate(LocaleKeys.Main.i_FireSystem)
            {
                Id = ItemId.FireSystem,
                Cost = 160,
                IconAddress = Addresses.Ico_FireSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(.8f, StatModificatorType.Additive, StatType.FireResist))//[[Огонь]] наносит вам на 80% урона меньше.
                }
            });
        AddItemTemplate( //Источник элегаза
            new ItemTemplate(LocaleKeys.Main.i_DielectricGasSource)
            {
                Id = ItemId.DielectricGasSource,
                Cost = 160,
                IconAddress = Addresses.Ico_DielectricGasSource,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerElectricityResist,
                    new StatModificator(.8f, StatModificatorType.Additive, StatType.ElectricityResist))//Дает 80% шанс уклониться от воздействия [[Электричество]].
                }
            });
        AddItemTemplate( //Балоны концентрированного отравляющего газа
            new ItemTemplate(LocaleKeys.Main.i_ConcentratedPoisonGasCylinders)
            {
                Id = ItemId.ConcentratedPoisonGasCylinders,
                Cost = 160,
                IconAddress = Addresses.Ico_ConcentratedPoisonGasCylinders,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Оставляет за собой  [[Ядовитый газ]].
                }
            });
        AddItemTemplate( //Устройство органического разложения
            new ItemTemplate(LocaleKeys.Main.i_OrganicDecompositionDevice)
            {
                Id = ItemId.OrganicDecompositionDevice,
                Cost = 160,
                IconAddress = Addresses.Ico_OrganicDecompositionDevice,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Врывает ядовитым облаком все подобранные хилки.
                }
            });
        AddItemTemplate( //Модуль насыщения кислородом
            new ItemTemplate(LocaleKeys.Main.i_OxygenSaturationModule)
            {
                Id = ItemId.OxygenSaturationModule,
                Cost = 160,
                IconAddress = Addresses.Ico_OxygenSaturationModule,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(8f, StatModificatorType.Additive, StatType.LifeSupport))//[[Запас кислорода]] + 8 с.
                }
            });
        AddItemTemplate( //Система лечения
            new ItemTemplate(LocaleKeys.Main.i_TreatmentSystem)
            {
                Id = ItemId.TreatmentSystem,
                Cost = 160,
                IconAddress = Addresses.Ico_TreatmentSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                    //[[Востановление хп]]  +1 раз в 16 с.
                }
            });
        AddItemTemplate( //Эхо
            new ItemTemplate(LocaleKeys.Main.i_Echo)
            {
                Id = ItemId.Echo,
                Cost = 160,
                IconAddress = Addresses.Ico_Echo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.MineralEffect,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.MineralEffect))//Удваевает эффекты минералов в ващем интвентаре.
                }
            });
        AddItemTemplate( //Самодельный огнемет
            new ItemTemplate(LocaleKeys.Main.i_HandmadeFlamethrower)
            {
                Id = ItemId.HandmadeFlamethrower,
                Cost = 160,
                IconAddress = Addresses.Ico_HandmadeFlamethrower,
                CreatePerksFunc = () => new IPerk[]
                {

                }
            });
        AddItemTemplate( //Белый саван
            new ItemTemplate(LocaleKeys.Main.i_WhiteShroud)
            {
                Id = ItemId.WhiteShroud,
                Cost = 40,
                IconAddress = Addresses.Ico_WhiteShroud,
                CreatePerksFunc = () => new IPerk[]
                {
                new PerkColdAOE(15),//При получении урона по персонажу выпускает волну [[Обморожение]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.FireResist))//[[Огонь]] наносит персонажу на 50 % урона меньше.
                }
            });
        AddItemTemplate( //Ледяная душа
            new ItemTemplate(LocaleKeys.Main.i_IceSoul)
            {
                Id = ItemId.IceSoul,
                Cost = 160,
                IconAddress = Addresses.Ico_IceSoul,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Накладывает [[Обморожение]] на 10 врагов в радиусе 40 п. раз в 10 с.
                }
            });
        AddItemTemplate( //Сброс ядовитых веществ
            new ItemTemplate(LocaleKeys.Main.i_PoisonousSubstancesDropping)
            {
                Id = ItemId.PoisonousSubstancesDropping,
                Cost = 160,
                IconAddress = Addresses.Ico_PoisonousSubstancesDropping,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Раз в 8 с. в случайную точку [[Зона контроля]] падает снаряд, выпускает волну [[Ядовитый газ]] радиусом 15 п.
                }
            });
        AddItemTemplate( //Сброс некондиционных материалов
            new ItemTemplate(LocaleKeys.Main.i_NonConformingMaterialsDropping)
            {
                Id = ItemId.NonConformingMaterialsDropping,
                Cost = 160,
                IconAddress = Addresses.Ico_NonConformingMaterialsDropping,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Раз в 8 с. в случайную точку [[Зона контроля]] падает снаряд, наносящий 60 ед.
                    //[[Физический урон]] в центре, и 20 ед. по бокам.
                    //Радиус поражения - 10 п.
                }
            });
        AddItemTemplate( //Доставка взрывоопасной смеси
            new ItemTemplate(LocaleKeys.Main.i_ExplosiveMixtureDropping)
            {
                Id = ItemId.ExplosiveMixtureDropping,
                Cost = 160,
                IconAddress = Addresses.Ico_ExplosiveMixtureDropping,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPBarrels>(8f),//Раз в 8 с. сбрасывает [[Ящик со взрывчаткой]].
                }
            });
        AddItemTemplate( //Доставка печеных яблок
            new ItemTemplate(LocaleKeys.Main.i_BakedApplesDelivery)
            {
                Id = ItemId.BakedApplesDelivery,
                Cost = 160,
                IconAddress = Addresses.Ico_BakedApplesDelivery,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Раз в 14 с. сбрасывает [[Ящик со взрывчаткой]].
                    //После взрыва оставляет после себя хилку.
                    //[[Востановление хп]] 1 ед.
                }
            });
        AddItemTemplate( //Радиационный контур
            new ItemTemplate(LocaleKeys.Main.i_RadiationContour)
            {
                Id = ItemId.RadiationContour,
                Cost = 160,
                IconAddress = Addresses.Ico_RadiationContour,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPIonization>(8f, 4f)//Пораженные лучем враги получают статус [[Радиация]] с уроном в 8 единицы.

                }
            });
        AddItemTemplate( //Изотопный реактор
            new ItemTemplate(LocaleKeys.Main.i_IsotopeReactor)
            {
                Id = ItemId.IsotopeReactor,
                Cost = 160,
                IconAddress = Addresses.Ico_IsotopeReactor,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(6, StatModificatorType.Additive, StatType.EnergyRegen))//[[Генерация энергии]] +6 ед./с.

                }
            });
        AddItemTemplate( //Атомный реактор малой мощьности
            new ItemTemplate(LocaleKeys.Main.i_SmallNuclearReactor)
            {
                Id = ItemId.SmallNuclearReactor,
                Cost = 160,
                IconAddress = Addresses.Ico_SmallNuclearReactor,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Генерация 5% от максимального заряда энергии в сек.

                }
            });
        AddItemTemplate( //Ионизатор атмосферы
            new ItemTemplate(LocaleKeys.Main.i_AtmosphericIonizer)
            {
                Id = ItemId.AtmosphericIonizer,
                Cost = 160,
                IconAddress = Addresses.Ico_AtmosphericIonizer,
                CreatePerksFunc = () => new IPerk[]
                {
                    //С каждой пройденым п. увеличивает появление молнии.

                }
            });
        AddItemTemplate( //Самозарядный аккумулятор
            new ItemTemplate(LocaleKeys.Main.i_SelfChargingAccumulator)
            {
                Id = ItemId.SelfChargingAccumulator,
                Cost = 160,
                IconAddress = Addresses.Ico_SelfChargingAccumulator,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.Capacity)),//[[Базовая ёмкость энергии]] + 60%.
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(3, StatModificatorType.Additive, StatType.EnergyRegen))//[[Генерация энергии]] +3 ед./ с.

                }
            });
        AddItemTemplate( //Контроллер перегрузки луча
            new ItemTemplate(LocaleKeys.Main.i_RayOverloadController)
            {
                Id = ItemId.RayOverloadController,
                Cost = 160,
                IconAddress = Addresses.Ico_RayOverloadController,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +50%.
                new SimplePerk(PerkType.RayDelay,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.RayDelay))//[[Задержка активации луча]] +1 секунды.

                }
            });
        AddItemTemplate( //Электроприводы
            new ItemTemplate(LocaleKeys.Main.i_ElectricDriveUnits)
            {
                Id = ItemId.ElectricDriveUnits,
                Cost = 160,
                IconAddress = Addresses.Ico_ElectricDriveUnits,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(1, StatModificatorType.Additive, StatType.RaySpeed)),//[[Базовая скорость луча]] +100%.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(2, StatModificatorType.Additive, StatType.RayError))//Погрешность +2п.
                }
            });
        AddItemTemplate( //Аккумулятор средней ёмкости
            new ItemTemplate(LocaleKeys.Main.i_MediumCapacityAccumulator)
            {
                Id = ItemId.MediumCapacityAccumulator,
                Cost = 160,
                IconAddress = Addresses.Ico_MediumCapacityAccumulator,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(1.2f, StatModificatorType.Multiplicative, StatType.Capacity))//[[Базовая ёмкость энергии]] + 120%.
                }
            });
        AddItemTemplate( //Корректор погрешностей
            new ItemTemplate(LocaleKeys.Main.i_ErrorCorrector)
            {
                Id = ItemId.ErrorCorrector,
                Cost = 160,
                IconAddress = Addresses.Ico_ErrorCorrector,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayError,
                              new StatModificator(-3, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] уменьшается на -3.
                                                                                                       //Уменьшает стоимсоть луча за пройденную длину луча.

                }
            });
        AddItemTemplate( //Хрустальное хало
            new ItemTemplate(LocaleKeys.Main.i_CrystalHalo)
            {
                Id = ItemId.CrystalHalo,
                Cost = 160,
                IconAddress = Addresses.Ico_CrystalHalo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(-.8f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] - 80%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(1.6f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] +160 %.

                }
            });
        AddItemTemplate( //Система рассеивания луча
            new ItemTemplate(LocaleKeys.Main.i_RayDiffusionSystem)
            {
                Id = ItemId.RayDiffusionSystem,
                Cost = 160,
                IconAddress = Addresses.Ico_RayDiffusionSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] + 80%.
                }
            });
        AddItemTemplate( //Система фокусировки луча
            new ItemTemplate(LocaleKeys.Main.i_RayFocusingSystem)
            {
                Id = ItemId.RayFocusingSystem,
                Cost = 160,
                IconAddress = Addresses.Ico_RayFocusingSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.4f, StatModificatorType.Multiplicative, StatType.RayDamage))//[[Базовый урон луча]] +40%.

                }
            });
        AddItemTemplate( //Хрустальная линза
            new ItemTemplate(LocaleKeys.Main.i_CrystalLens)
            {
                Id = ItemId.CrystalLens,
                Cost = 160,
                IconAddress = Addresses.Ico_CrystalLens,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.8f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +80%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(-0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)),//[[Радиус поражения луча]] -80 %.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] +1п.

                }
            });
        AddItemTemplate( //Огненные следы
            new ItemTemplate(LocaleKeys.Main.i_FireFootprints)
            {
                Id = ItemId.FireFootprints,
                Cost = 160,
                IconAddress = Addresses.Ico_FireFootprints,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Накапливает заряд, следующий луч оставляет огненный след.


                }
            });
        ///////////////////////////////////////////////////////////////////////////////////////////красные арты
        AddItemTemplate( //Покров разложения
            new ItemTemplate(LocaleKeys.Main.i_VeilOfDecay)
            {
                Id = ItemId.VeilOfDecay,
                Cost = 320,
                IconAddress = Addresses.Ico_VeilOfDecay,
                CreatePerksFunc = () => new IPerk[]
                {
                //Врывает ядовитым облаком все подобранные хилки.
                //При получении урона выпускает волну [[Ядовитый газ]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerMaxHP,
                               new StatModificator(5f, StatModificatorType.Additive, StatType.MaxHP))//[[Мак хит поинтов]] + 5 ед.


                }
            });
        AddItemTemplate( //Экзоскелет война
            new ItemTemplate(LocaleKeys.Main.i_WarriorExoskeleton)
            {
                Id = ItemId.WarriorExoskeleton,
                Cost = 320,
                IconAddress = Addresses.Ico_WarriorExoskeleton,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerStunResist,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.StunResist)),//[[Оглушение]] не действует на персонажа.
                new SimplePerk(PerkType.PlayerMaxHP,
                               new StatModificator(10f, StatModificatorType.Additive, StatType.MaxHP))//Макс хп + 10.
                                                                                                      //[[Экзоскелет]].



                }
            });
        AddItemTemplate( //Система сдвига пространства
            new ItemTemplate(LocaleKeys.Main.i_SpaceShiftSystem)
            {
                Id = ItemId.SpaceShiftSystem,
                Cost = 320,
                IconAddress = Addresses.Ico_SpaceShiftSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                //После получаения урона персонаж получает статус [[Имматериальный]] на 3 с.
                //Наносимый персонажу урон увеличивается на 1.
                new SimplePerk(PerkType.Speed,
                               new StatModificator(1.6f, StatModificatorType.Additive, StatType.MovementSpeed))//[[Скорость передвижения]] +1.6 п/с.



                }
            });
        AddItemTemplate( //Экзоскелет пионера
            new ItemTemplate(LocaleKeys.Main.i_PioneerExoskeleton)
            {
                Id = ItemId.PioneerExoskeleton,
                Cost = 320,
                IconAddress = Addresses.Ico_PioneerExoskeleton,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.Speed,
                               new StatModificator(1.6f, StatModificatorType.Additive, StatType.MovementSpeed)),//[[Скорость передвижения]] +1.6 п/с.
                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(8f, StatModificatorType.Additive, StatType.LifeSupport))//[[Запас кислорода]] +8 с.
                                                                                                           //[[Экзоскелет]].




                }
            });
        AddItemTemplate( //Устройство гравитационного взрыва материи
            new ItemTemplate(LocaleKeys.Main.i_GravitationalExplosionDevice)
            {
                Id = ItemId.GravitationalExplosionDevice,
                Cost = 320,
                IconAddress = Addresses.Ico_GravitationalExplosionDevice,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Подберая руду, она взрывается на 8 осколков, летящих на 30 п. и наносяших 40 ед.  [[Физический урон]].



                }
            });
        AddItemTemplate( //Вечный двигатель
            new ItemTemplate(LocaleKeys.Main.i_PerpetualMotionMachine)
            {
                Id = ItemId.PerpetualMotionMachine,
                Cost = 320,
                IconAddress = Addresses.Ico_PerpetualMotionMachine,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Раз в 3 с. выпускает в случайного врага молнию, бьющую 20 ед. [[Электричество]].



                }
            });
        AddItemTemplate( //Звездный спутник
            new ItemTemplate(LocaleKeys.Main.i_StarSatellite)
            {
                Id = ItemId.StarSatellite,
                Cost = 320,
                IconAddress = Addresses.Ico_StarSatellite,
                CreatePerksFunc = () => new IPerk[]
                {
                new PerkGravityMatter(25, 1, 40, 2, 2, PerkType.GravityMatter)//Вращает осколок материи вокруг персонажа в радиусе 25 п.
                                                                              //Осколок наносит 40 ед.  [[Физический урон]] и оглушает на 2 секунды.



                }
            });
        AddItemTemplate( //Экзоскелет харвестера
            new ItemTemplate(LocaleKeys.Main.i_HarvesterExoskeleton)
            {
                Id = ItemId.HarvesterExoskeleton,
                Cost = 320,
                IconAddress = Addresses.Ico_HarvesterExoskeleton,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(3f, StatModificatorType.Multiplicative, StatType.PickupRadius)),//[[Радиус сбора]] + 300%.
                new SimplePerk(PerkType.PlayerSlowdownResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.SlowdownResist))//Жижа не замедляет
                                                                                                             //Жижа растворяется
                                                                                                             //[[Экзоскелет]].



                }
            });
        AddItemTemplate( //Система контроля окружающей среды
            new ItemTemplate(LocaleKeys.Main.i_EnvironmentalControlSystem)
            {
                Id = ItemId.EnvironmentalControlSystem,
                Cost = 320,
                IconAddress = Addresses.Ico_EnvironmentalControlSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerSlowdownResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.SlowdownResist)),//Жижа не замедляет
                //Жижа растворяется

                new SimplePerk(PerkType.PlayerFireResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.FireAbsorption))//[[Огонь]] не наносит урона.



                }
            });
        AddItemTemplate( //Экзоскелет инженера
            new ItemTemplate(LocaleKeys.Main.i_EngineerExoskeleton)
            {
                Id = ItemId.EngineerExoskeleton,
                Cost = 320,
                IconAddress = Addresses.Ico_EngineerExoskeleton,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PlayerElectricityResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.ElectricityResist)),//[[Электричество]] не воздействует на персонажа
                new SimplePerk(PerkType.PlayerRadiationResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.RadiationResist))//[[Радиация]] не воздействует на персонажа.
                                                                                                              //[[Экзоскелет]].


                }
            });
        AddItemTemplate( //Система регенерации тканей
            new ItemTemplate(LocaleKeys.Main.i_TissueRegenerationSystem)
            {
                Id = ItemId.TissueRegenerationSystem,
                Cost = 40,
                IconAddress = Addresses.Ico_TissueRegenerationSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                    //[[Востановление хп]]  +1 раз в 8 с.



                }
            });
        AddItemTemplate( //Боевой огнемет
            new ItemTemplate(LocaleKeys.Main.i_CombatFlamethrower)
            {
                Id = ItemId.CombatFlamethrower,
                Cost = 320,
                IconAddress = Addresses.Ico_CombatFlamethrower,
                CreatePerksFunc = () => new IPerk[]
                {




                }
            });
        AddItemTemplate( //Система жизнеобеспечения
            new ItemTemplate(LocaleKeys.Main.i_LifeSupportingSystem)
            {
                Id = ItemId.LifeSupportingSystem,
                Cost = 320,
                IconAddress = Addresses.Ico_LifeSupportingSystem,
                CreatePerksFunc = () => new IPerk[]
                {

                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(8f, StatModificatorType.Additive, StatType.LifeSupport))//[[Запас кислорода]] + 8 с.
                                                                                                           //[[Востановление хп]]  +1 раз в 16 с.


                }
            });
        AddItemTemplate( //Великое оледенение
            new ItemTemplate(LocaleKeys.Main.i_GreatGlaciation)
            {
                Id = ItemId.GreatGlaciation,
                Cost = 320,
                IconAddress = Addresses.Ico_GreatGlaciation,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Накладывает [[Обморожение]] на 20 врагов в радиусе 50 п. раз в 10 с.


                }
            });
        AddItemTemplate( //Экзоскелет тактика
            new ItemTemplate(LocaleKeys.Main.i_TacticExoskeleton)
            {
                Id = ItemId.TacticExoskeleton,
                Cost = 320,
                IconAddress = Addresses.Ico_TacticExoskeleton,
                CreatePerksFunc = () => new IPerk[]
                {

                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(10f, StatModificatorType.Additive, StatType.MaxHP)),//[[Мак хит поинтов]] + 10 ед.
                //При получении урона по персонажу выпускает волну [[Обморожение]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerFireResist,
                               new StatModificator(.5f, StatModificatorType.Additive, StatType.FireAbsorption))//[[Огонь]] не наносит урона.//[[Огонь]] наносит персонажу на 50% урона меньше.
                                                                                                               //[[Экзоскелет]].


                }
            });
        AddItemTemplate( //Полная разгрузка
            new ItemTemplate(LocaleKeys.Main.i_FullUnloading)
            {
                Id = ItemId.FullUnloading,
                Cost = 320,
                IconAddress = Addresses.Ico_FullUnloading,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Раз в 4 с. в случайную точку [[Зона контроля]] падает снаряд, выпускает волну [[Ядовитый газ]] радиусом 15 п.


                }
            });
        AddItemTemplate( //Метеоритный дождь
            new ItemTemplate(LocaleKeys.Main.i_MeteorRain)
            {
                Id = ItemId.MeteorRain,
                Cost = 320,
                IconAddress = Addresses.Ico_MeteorRain,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Раз в 4 с. в случайную точку [[Зона контроля]] падает снаряд, наносящий 60 ед. [[Физический урон]] в центре, и 20 ед. по бокам.
                    //Радиус поражения - 10 п.


                }
            });
        AddItemTemplate( //Арсенал
            new ItemTemplate(LocaleKeys.Main.i_Arsenal)
            {
                Id = ItemId.Arsenal,
                Cost = 320,
                IconAddress = Addresses.Ico_Arsenal,
                CreatePerksFunc = () => new IPerk[]
                {

                _perksFactory.Create<PerkPBarrels>(4f),//Раз в 4 с. сбрасывает [[Ящик со взрывчаткой]].


                }
            });
        AddItemTemplate( //Система орбитальной поддержки
            new ItemTemplate(LocaleKeys.Main.i_OrbitalSupportSystem)
            {
                Id = ItemId.OrbitalSupportSystem,
                Cost = 320,
                IconAddress = Addresses.Ico_OrbitalSupportSystem,
                CreatePerksFunc = () => new IPerk[]
                {

                new SimplePerk(PerkType.RayError,
                              new StatModificator(-4, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] уменьшается на -4.
                                                                                                       //Уменьшает стоимсоть луча за пройденную длину луча.
                                                                                                       //Раз в 14 с. сбрасывает [[Ящик со взрывчаткой]].
                                                                                                       //После взрыва оставляет после себя хилку.
                                                                                                       //[[Востановление хп]] 1 ед.


                }
            });
        AddItemTemplate( //Атомный луч
            new ItemTemplate(LocaleKeys.Main.i_AtomicRay)
            {
                Id = ItemId.AtomicRay,
                Cost = 320,
                IconAddress = Addresses.Ico_AtomicRay,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Луч прекрощяет бить фотонным уроном.
                    //Он накладывает статус [[Радиация]] с уроном в 50% от [[Базовый урон луча]].


                }
            });
        AddItemTemplate( //Перегруженный луч
            new ItemTemplate(LocaleKeys.Main.i_OverloadedRay)
            {
                Id = ItemId.OverloadedRay,
                Cost = 320,
                IconAddress = Addresses.Ico_OverloadedRay,
                CreatePerksFunc = () => new IPerk[]
                {

                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +100%.
                new SimplePerk(PerkType.RayDelay,
                               new StatModificator(2, StatModificatorType.Additive, StatType.RayDelay))//[[Задержка активации луча]] +2 секунды.


                }
            });
        AddItemTemplate( //Хемоядерный реактор
            new ItemTemplate(LocaleKeys.Main.i_ChemonuclearReactor)
            {
                Id = ItemId.ChemonuclearReactor,
                Cost = 320,
                IconAddress = Addresses.Ico_ChemonuclearReactor,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Генерация 10% от максимального заряда энергии в сек.


                }
            });
        AddItemTemplate( //Молния
            new ItemTemplate(LocaleKeys.Main.i_Lightning)
            {
                Id = ItemId.Lightning,
                Cost = 320,
                IconAddress = Addresses.Ico_Lightning,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Луч прекрощяет бить фотонным уроном.


                }
            });
        AddItemTemplate( //Аккумулятор большой ёмкости
            new ItemTemplate(LocaleKeys.Main.i_HighCapacityAccumulator)
            {
                Id = ItemId.HighCapacityAccumulator,
                Cost = 320,
                IconAddress = Addresses.Ico_HighCapacityAccumulator,
                CreatePerksFunc = () => new IPerk[]
                {

                new SimplePerk(PerkType.RayPathLenght,
                               new StatModificator(2f, StatModificatorType.Multiplicative, StatType.Capacity))//[[Базовая ёмкость энергии]] + 200%.


                }
            });
        AddItemTemplate( //Экстраприводы
            new ItemTemplate(LocaleKeys.Main.i_ExtraDriveUnits)
            {
                Id = ItemId.ExtraDriveUnits,
                Cost = 320,
                IconAddress = Addresses.Ico_ExtraDriveUnits,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(2, StatModificatorType.Additive, StatType.RaySpeed),//[[Базовая скорость луча]] +200%.
                               new StatModificator(3, StatModificatorType.Additive, StatType.RayError)) //Погрешность +3п.

                }
            });
        AddItemTemplate( //Система обратного хода
            new ItemTemplate(LocaleKeys.Main.i_ReverseSystemTwo)
            {
                Id = ItemId.ReverseSystemTwo,
                Cost = 320,
                IconAddress = Addresses.Ico_ReverseSystemTwo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(1.3f, StatModificatorType.Additive, StatType.RayError)),//[[Базовая скорость луча]] +130%.
                new SimplePerk(PerkType.RayReverse,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.RayReverse))//Луч проходит свой путь дважды.
                }
            });
        AddItemTemplate( //Столб света
            new ItemTemplate(LocaleKeys.Main.i_PillarOfLight)
            {
                Id = ItemId.PillarOfLight,
                Cost = 320,
                IconAddress = Addresses.Ico_PillarOfLight,
                CreatePerksFunc = () => new IPerk[]
                {

                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(-.9f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] - 90%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(2.5f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] +250 %


                }
            });
        AddItemTemplate( //Расщепитель луча
            new ItemTemplate(LocaleKeys.Main.i_RaySplitter)
            {
                Id = ItemId.RaySplitter,
                Cost = 320,
                IconAddress = Addresses.Ico_RaySplitter,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Создает 3 луча вместо одного, [[Базовый урон луча]] которых равен 40%, а [[Радиус поражения луча]]  60%.
                    //Все дополнительные параметры применяются ко всем 3м лучам.


                }
            });
        AddItemTemplate( //Зеркало Архимеда
            new ItemTemplate(LocaleKeys.Main.i_ArchimedesMirror)
            {
                Id = ItemId.ArchimedesMirror,
                Cost = 320,
                IconAddress = Addresses.Ico_ArchimedesMirror,
                CreatePerksFunc = () => new IPerk[]
                {

                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(1.2f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +120%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(-0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)),//[[Радиус поражения луча]] -80 %.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] +1п.


                }
            });
        AddItemTemplate( //Огненный луч
            new ItemTemplate(LocaleKeys.Main.i_FireRay)
            {
                Id = ItemId.FireRay,
                Cost = 320,
                IconAddress = Addresses.Ico_FireRay,
                CreatePerksFunc = () => new IPerk[]
                {

                    //Луч прекрощяет бить фотонным уроном.
                    //Он бьет [[Огонь]] с уроном в 50% от [[Базовый урон луча]].


                }
            });

        /// ///////////////////////////////////////////////////////////////////////////////////////белые арты


        AddItemTemplate( //Пустышка
            new ItemTemplate(LocaleKeys.Main.i_Empty)
            {
                Id = ItemId.Empty,
                Cost = 40,
                IconAddress = Addresses.Ico_Empty,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Ничего не делает.
                }
            });
        AddItemTemplate( //Огненный самоцвет
            new ItemTemplate(LocaleKeys.Main.i_FireGem)
            {
                Id = ItemId.FireGem,
                Cost = 40,
                IconAddress = Addresses.Ico_FireGem,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Раз в 10 секунды создает 5 искр, наносяжих 20 ед. урона [[Огонь]].
                    //Искры летят от персонажа на растояние в 40 п.
                }
            });
        AddItemTemplate( //Лучистый минерал
            new ItemTemplate(LocaleKeys.Main.i_RadiantMineral)
            {
                Id = ItemId.RadiantMineral,
                Cost = 40,
                IconAddress = Addresses.Ico_RadiantMineral,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Накладывает статус [[Радиация]] в радиусе 10 п. от персонажа с уроном в 4 единицы  раз в 2 с. в течение 8 с.
                }
            });
        AddItemTemplate( //Искажающий хрусталь
            new ItemTemplate(LocaleKeys.Main.i_DistortingCrystal)
            {
                Id = ItemId.DistortingCrystal,
                Cost = 40,
                IconAddress = Addresses.Ico_DistortingCrystal,
                CreatePerksFunc = () => new IPerk[]
                {
                    //Наносимый персонажу урон увеличивается на 1.
                    //[[Минерал]].
                }
            });
        AddItemTemplate( //Электрическая жила
            new ItemTemplate(LocaleKeys.Main.i_ElectricalLead)
            {
                Id = ItemId.ElectricalLead,
                Cost = 40,
                IconAddress = Addresses.Ico_ElectricalLead,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkElectricalLead>(6f)//Раз в 6 с. выпускает в случайного врага молнию, бьющую 20 ед. [[Электричество]].
                                                            //[[Минерал]].
                }
            });
        AddItemTemplate( //Гравитационный камень
            new ItemTemplate(LocaleKeys.Main.i_GravityStone)
            {
                Id = ItemId.GravityStone,
                Cost = 40,
                IconAddress = Addresses.Ico_GravityStone,
                CreatePerksFunc = () => new IPerk[]
                {
                new PerkGravityMatter(15, 2, 10, .5f, 1, PerkType.GravityMatter)
                    //Вращает осколок материи вокруг персонажа в радиусе 15 п. Осколок наносит 10 ед.урона [[Физический урон]] и оглушает на 0.5 секунды.
                    //[[Ксеноминерал]].
                }
            });
        AddItemTemplate( //Фиолетовый клубешь
            new ItemTemplate(LocaleKeys.Main.i_PurpleTuber)
            {
                Id = ItemId.PurpleTuber,
                Cost = 40,
                IconAddress = Addresses.Ico_PurpleTuber,
                CreatePerksFunc = () => new IPerk[]
                {
                    //При получении урона по персонажу выпускает волну [[Ядовитый газ]] радиусом 20 п.
                    //[[Минерал]].
                }
            });
        AddItemTemplate( //Полюс холода
            new ItemTemplate(LocaleKeys.Main.i_PoleOfCold)
            {
                Id = ItemId.PoleOfCold,
                Cost = 40,
                IconAddress = Addresses.Ico_PoleOfCold,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkColdAOEWithCooldown>(5, 40f, 10f)
                    //new PerkColdAOE(5, 40, 10)
                    //Накладывает [[Обморожение]] на 5 врагов в радиусе 40 п. раз в 10 с.
                    //[[Минерал]].
                }
            });
        AddItemTemplate( //Каменное яблоко
            new ItemTemplate(LocaleKeys.Main.i_StoneApple)
            {
                Id = ItemId.StoneApple,
                Cost = 40,
                IconAddress = Addresses.Ico_StoneApple,
                CreatePerksFunc = () => new IPerk[]
                {
                    //[[Востановление хп]]  +1 раз в 30 с.
                    //[[Минерал]].
                }
            });
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Battery)
            {
                Id = ItemId.Battery,
                Cost = 30,
                IconAddress = Addresses.Ico_Battery,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayPathLenght,
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.Capacity))
                },
            });

        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_AtomicBattery)
            {
                Id = ItemId.AtomicBattery,
                Cost = 50,
                IconAddress = Addresses.Ico_AtomicBattery,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyRegen,
                    new StatModificator(2, StatModificatorType.Additive, StatType.EnergyRegen))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_AtomicBattery)
            {
                UpgradeCount = 1,
                Id = ItemId.AtomicBatteryPlus,
                Cost = 100,
                IconAddress = Addresses.Ico_AtomicBattery,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyRegen,
                    new StatModificator(4, StatModificatorType.Additive, StatType.EnergyRegen))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_AtomicBattery)
            {
                UpgradeCount = 1,
                Id = ItemId.AtomicBatteryPlusPlus,
                Cost = 250,
                IconAddress = Addresses.Ico_AtomicBattery,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.EnergyRegen,
                    new StatModificator(8, StatModificatorType.Additive, StatType.EnergyRegen))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Battery)
            {
                Id = ItemId.BatteryPlus,
                UpgradeCount = 1,
                Cost = 75,
                IconAddress = Addresses.Ico_Battery,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayPathLenght,
                    new StatModificator(.75f, StatModificatorType.Multiplicative, StatType.Capacity))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Battery)
            {
                Id = ItemId.BatteryPlusPlus,
                UpgradeCount = 2,
                Cost = 150,
                IconAddress = Addresses.Ico_Battery,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayPathLenght,
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.Capacity))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_AdditionalDrives)
            {
                Id = ItemId.AdditionalDrives,
                Cost = 50,
                IconAddress = Addresses.Ico_Servo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RaySpeed,
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RaySpeed))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_ExoskeletonSpeedBooster)
            {
                Id = ItemId.ExoskeletonSpeedBooster,
                Cost = 60,
                IconAddress = Addresses.Ico_SpeedBoost,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.Speed,
                    new StatModificator(0.1f, StatModificatorType.Multiplicative, StatType.MovementSpeed))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Amplifier)
            {
                Id = ItemId.Amplifier,
                Cost = 50,
                IconAddress = Addresses.Ico_Amplifier,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.Amplifier,
                    new StatModificator(-0.2f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(0.2f, StatModificatorType.Multiplicative, StatType.RayDamage))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Amplifier)
            {
                UpgradeCount = 1,
                Id = ItemId.AmplifierPlus,
                Cost = 150,
                IconAddress = Addresses.Ico_Amplifier,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.Amplifier,
                    new StatModificator(-0.3f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(0.4f, StatModificatorType.Multiplicative, StatType.RayDamage))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Amplifier)
            {
                UpgradeCount = 2,
                Id = ItemId.AmplifierPlusPlus,
                Cost = 400,
                IconAddress = Addresses.Ico_Amplifier,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.Amplifier,
                    new StatModificator(-0.4f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamage))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_FocusLens)
            {
                Id = ItemId.FocusLens,
                Unique = true,
                Cost = 70,
                IconAddress = Addresses.Ico_Lens,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.FocusLens,
                    new StatModificator(-0.9f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.RayDamage))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_EnergyAbsorber)
            {
                Id = ItemId.EnergyAbsorber,
                Cost = 60,
                IconAddress = Addresses.Ico_Laser,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPEnergyAbsorber>(5f)//урон, длительность
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_IonizationUnit, LocaleKeys.Main.id_IonizationUnit)
            {
                Id = ItemId.IonizationUnit,
                Cost = 60,
                IconAddress = Addresses.Ico_Radiation,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPIonization>(4f, 10f)//урон, длительность
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_IonizationUnit, LocaleKeys.Main.id_IonizationUnitP)
            {
                UpgradeCount = 1,
                Id = ItemId.IonizationUnitPlus,
                Cost = 180,
                IconAddress = Addresses.Ico_Radiation,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPIonization>(6f, 15f)//урон, длительность
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_IonizationUnit, LocaleKeys.Main.id_IonizationUnitPP)
            {
                UpgradeCount = 2,
                Id = ItemId.IonizationUnitPlusPlus,
                Cost = 480,
                IconAddress = Addresses.Ico_Radiation,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPIonization>(12f, 20f)//урон, длительность
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_ShiftSystem, LocaleKeys.Main.id_ShiftSystem)
            {
                Unique = true,
                Id = ItemId.ShiftSystem,
                Cost = 100,
                IconAddress = Addresses.Ico_ShiftSystem,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPShiftSystem>(),
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_DeliveryDevice, LocaleKeys.Main.id_DeliveryDevice)
            {
                Id = ItemId.DeliveryDevice,
                Cost = 50,
                IconAddress = Addresses.Ico_Box,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPBarrels>(20f),//cooldown
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_DeliveryDevice, LocaleKeys.Main.id_DeliveryDeviceP)
            {
                UpgradeCount = 1,
                Id = ItemId.DeliveryDevicePlus,
                Cost = 150,
                IconAddress = Addresses.Ico_Box,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPBarrels>(15f),//cooldown
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_DeliveryDevice, LocaleKeys.Main.id_DeliveryDevicePP)
            {
                UpgradeCount = 2,
                Id = ItemId.DeliveryDevicePlusPlus,
                Cost = 400,
                IconAddress = Addresses.Ico_Box,
                CreatePerksFunc = () => new IPerk[]
                {
                _perksFactory.Create<PerkPBarrels>(10f),//cooldown
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Coprocessor)
            {
                Id = ItemId.Coprocessor,
                Cost = 50,
                IconAddress = Addresses.Ico_Chip,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDelay,
                    new StatModificator(-.3f, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-1f, StatModificatorType.Additive, StatType.RayError))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Coprocessor)
            {
                UpgradeCount = 1,
                Id = ItemId.CoprocessorPlus,
                Cost = 150,
                IconAddress = Addresses.Ico_Chip,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDelay,
                    new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-2f, StatModificatorType.Additive, StatType.RayError))
                },
            });

        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_Coprocessor)
            {
                UpgradeCount = 2,
                Id = ItemId.CoprocessorPlusPlus,
                Cost = 400,
                IconAddress = Addresses.Ico_Chip,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayDelay,
                    new StatModificator(-1, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-4f, StatModificatorType.Additive, StatType.RayError))
                },
            });

        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_DivergingLens)
            {
                Id = ItemId.DivergingLens,
                Unique = false,
                Cost = 60,
                IconAddress = Addresses.Ico_DivergingLens,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.DivergingLens,
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(-0.5f, StatModificatorType.Multiplicative, StatType.RayDamage))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_PowerController)
            {
                Id = ItemId.PowerController,
                Cost = 100,
                IconAddress = Addresses.Ico_PowerController,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PowerController,
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayDamage))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_PowerController)
            {
                UpgradeCount = 1,
                Id = ItemId.PowerControllerPlus,
                Cost = 200,
                IconAddress = Addresses.Ico_PowerController,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PowerController,
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RayDamage))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_PowerController)
            {
                UpgradeCount = 2,
                Id = ItemId.PowerControllerPlusPlus,
                Cost = 500,
                IconAddress = Addresses.Ico_PowerController,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.PowerController,
                    new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.Capacity),
                new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.RayDamage))
                },
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_LensSystem)
            {
                Id = ItemId.LensSystem,
                Unique = false,
                Cost = 150,
                IconAddress = Addresses.Ico_Lenses,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.LensSystem,
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_LensSystem)
            {
                UpgradeCount = 1,
                Id = ItemId.LensSystemPlus,
                Unique = false,
                Cost = 400,
                IconAddress = Addresses.Ico_Lenses,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.LensSystem,
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_MagneticManipulator)
            {
                Id = ItemId.MagneticManipulator,
                Unique = false,
                Cost = 40,
                IconAddress = Addresses.Ico_Magnet,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.PickupRadius))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_ReverseSystem, LocaleKeys.Main.id_ReverseSystem)
            {
                Id = ItemId.ReverseSystem,
                Cost = 300,
                IconAddress = Addresses.Ico_Reverse,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RayReverse,
                    new StatModificator(1f, StatModificatorType.Additive, StatType.RayReverse),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed))
                }
            });
        AddItemTemplate(
            new ItemTemplate(LocaleKeys.Main.i_SpeedDrives)
            {
                UpgradeCount = 1,
                Id = ItemId.SpeedDrives,
                Cost = 100,
                IconAddress = Addresses.Ico_Servo,
                CreatePerksFunc = () => new IPerk[]
                {
                new SimplePerk(PerkType.RaySpeed,
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed),
                    new StatModificator(3, StatModificatorType.Additive, StatType.RayError))
                }
            });
    }
}
