using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Collections;
using System.Linq;

namespace Okiroya.Campione.DataAccess
{
    public abstract class TableValueParameter<T> : DbDataReader
    {
        private T[] _data;
        private Dictionary<string, string> _columnMapping;
        private int _index;

        public bool HasColumnMapping => _columnMapping != null;

        public TableValueParameter(Dictionary<string, string> columnMapping = null)
        {
            _columnMapping = columnMapping;
        }

        public TableValueParameter<T> AddData(IEnumerable<T> data)
        {
            Guard.ArgumentNotNull(data);

            _data = data.ToArray();

            return this;
        }

        public IEnumerator<KeyValuePair<string, string>> GetColumnMappingEnumerator()
        {
            if (_columnMapping == null)
            {
                yield break;
            }

            foreach (var item in _columnMapping)
            {
                yield return item;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return GetColumnMappingEnumerator();
        }

        #region BulkCopy methods

        public override int FieldCount => _data.Length;

        public override bool HasRows => FieldCount > 0;

        public override bool Read()
        {
            return _index++ < FieldCount;
        }

        public override object GetValue(int i)
        {
            return i < FieldCount ?
                (object)_data[i] :
                null;
        }

        #endregion

        #region DbDataReader methods

        public override int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsClosed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int RecordsAffected
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override object this[string name]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override object this[int ordinal]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
