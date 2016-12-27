using System;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Объект-контейнер, кортеж. Служит хранилищем мета информации о поле сущности из БД
    /// </summary>
    
    public class DataItemElement : Tuple<string, Type, object>
    {
        /// <summary>
        /// Инициализация кортежа 3-мя элементами
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <param name="itemValue"></param>
        public DataItemElement(string itemName, Type itemType, object itemValue)
            : base(itemName, itemType, itemValue)
        { }

        /// <summary>
        /// Инициализация кортежа другим кортежем
        /// </summary>
        /// <param name="proto"></param>
        public DataItemElement(Tuple<string, Type, object> proto)
            : this(proto.Item1, proto.Item2, proto.Item3)
        { }

        /// <summary>
        /// Имя поля
        /// </summary>
        public string ItemName
        {
            get
            {
                return Item1;
            }
        }

        /// <summary>
        /// Тип поля
        /// </summary>
        public Type ItemType
        {
            get
            {
                return Item2;
            }
        }

        /// <summary>
        /// Значения поля
        /// </summary>
        public object ItemValue
        {
            get
            {
                return Item3;
            }
        }
    }
}
