using System;
using System.Collections.Generic;
using System.Collections;

namespace Okiroya.Campione.Service.Paging
{
    /// <summary>
    /// Коллекция, которую можно читать постранично. Содержит типизированные элементы
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    public class PagedCollection<T> : PagedCollection, IPagedCollection, IList<T>
    {
        private IList<T> _list;

        #region Constructors

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public PagedCollection()
            : base()
        {
            _list = new List<T>();
        }

        /// <summary>
        /// Конструктор, принимающий инициализирующую коллекцию
        /// </summary>
        /// <param name="source"></param>
        public PagedCollection(IEnumerable<T> source)
        {
            _list = new List<T>(source);
        }

        #endregion

        #region IList implementation

        /// <summary>
        /// Вернуть индекс элемента
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Вставить элемент в коллекция в определенное индексом место
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        /// <summary>
        /// Удалить элемент из коллекции
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// Очистить коллекцию
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Содержит ли коллекция элемент
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Скопировать коллекцию в массив
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        /// <summary>
        /// Коллекция достпна только для чтения или нет
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _list.IsReadOnly;
            }
        }

        /// <summary>
        /// Удалить элемент
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return _list.Remove(item);
        }

        /// <summary>
        /// Вернуть итератор
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PagedCollectionEnumerator(this);
        }

        #endregion

        private struct PagedCollectionEnumerator : IEnumerator<T>
        {
            private IEnumerator _enumerator;

            public PagedCollectionEnumerator(PagedCollection<T> list)
            {
                _enumerator = list.GetEnumerator();
            }

            public T Current
            {
                get
                {
                    return (T)_enumerator.Current;
                }
            }

            public void Dispose()
            {
                _enumerator.Reset();

                _enumerator = null;
            }

            object IEnumerator.Current
            {
                get
                {
                    return _enumerator.Current;
                }
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}
