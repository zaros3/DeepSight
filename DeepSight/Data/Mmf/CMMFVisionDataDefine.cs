using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 메모리맵 페이지의 정의가 있는 네임스페이스 입니다.
/// </summary>
namespace ENC.MemoryMap.VisionDefine
{
    using System.IO;
    using System.IO.MemoryMappedFiles;

    // [메모리맵 페이지 베이스 클래스]
    //
    // 1. MMData_Base는 고정된 **1Page의 크기를 가진다.
    // 2. 각각의 데이터 형에 배열 처럼 접근하여 사용 할 수 있다.
    // 3. MMData_Base를 상속 받은 클래스에는 프로퍼티를 정의하여 배열 접근에 용이하게 한다.
    // 4. 메모리 맵 파일 클래스를 생성 할 때는 CreateMemMap() 함수를 이용하여 만든다.
    // 
    // **1Page: MMData_Base에 정의된 데이터 크기를 의미하며 고정된 크기를 가진다.
    //

    /// <summary>
    /// 메모리 맵 베이스 클래스 입니다.
    /// 이 클래스를 상속 받으면 고정된 크기의 메모리 블럭을 생성 합니다.
    /// </summary>
    class MMVisionData_PageBase : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 클래스 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 바이트형 데이터 클래스 입니다.
        /// </summary>
        private MMData_ByteData _byteData;
        /// <summary>
        /// 불형 데이터 클래스 입니다.
        /// </summary>
        private MMData_BoolData _boolData;
        /// <summary>
        /// 쇼트형 데이터 클래스 입니다.
        /// </summary>
        private MMData_ShortData _shortData;
        /// <summary>
        /// 인트형 데이터 클래스 입니다.
        /// </summary>
        private MMData_IntData _intData;
        /// <summary>
        /// 플롯형 데이터 클래스 입니다.
        /// </summary>
        private MMData_FloatData _floatData;
        /// <summary>
        /// 더블형 데이터 클래스 입니다.
        /// </summary>
        private MMData_DoubleData _doubleData;
        /// <summary>
        /// 스트링형 데이터 클래스 입니다.
        /// </summary>
        private MMData_StringData _stringData;

        /// <summary>
        /// 바이트형 데이터의 크기 입니다.
        /// </summary>
        public static readonly long ByteDataSize = 1024 * 1024;        //=  15 MegaByte
        /// <summary>
        /// 바이트형 데이터의 크기 입니다.
        /// </summary>
        public static readonly long BoolDataSize = 2048 * 1;        //=  2,048 byte
        /// <summary>
        /// 쇼트형 데이터의 크기 입니다.
        /// </summary>
        public static readonly long ShortDataSize = 512 * 2;          //=  327,676 byte
        /// <summary>
        /// 인트형 데이터의 크기 입니다.
        /// </summary>
        public static readonly long IntDataSize = 512 * 4;          //=  2,048 byte
        /// <summary>
        /// 플롯형 데이터의 크기 입니다.
        /// </summary>
        public static readonly long FloatDataSize = 512 * 4;        //=  2,048 byte
        /// <summary>
        /// 더블형 데이터의 크기 입니다.
        /// </summary>
        public static readonly long DoubleDataSize = 512 * 8;        //=  4,096 byte
        /// <summary>
        /// 스트링형 데이터의 크기 입니다.
        /// </summary>
        public static readonly long StringDataSize = 128 * 512 * 1; //= 65,536 byte
        /// <summary>
        /// 메모리 블럭 하나의 크기 입니다.
        /// </summary>
        public static readonly long PageSize = ( 1024 * 1024 ) + ( 2048 * 1 ) + ( 512 * 2 ) + ( 512 * 4 ) + ( 512 * 4 ) + ( 512 * 8 ) + ( 128 * 512 * 1 ); // = ByteDataSize + IntDataSize + FloatDataSize + DoubleDataSize + StringDataSize

        /// <summary>
        /// 메모리 맵 베이스의 생성자 입니다.
        /// </summary>
        /// <param name="fileName">메모리맵 파일 이름. 기존에 같은 이름의 파일이 존재하지 않으면 만들고, 있으면 파일을 엽니다.</param>
        /// <param name="pageIndex">메모리 블럭 인덱스. 메모리 블럭의 인덱스 입니다. 인덱스가 메모리 블럭 보다 커지면 익셉션을 발생시킵니다.</param>
        /// <param name="pageCount">메모리 블럭 개수. 메모리 블럭 개수를 참고하여 메모리 맵 파일을 생성합니다. 메모리 블럭은 0보다 커야합니다.</param>
        protected MMVisionData_PageBase(MemoryMappedFile memMap, uint pageIndex, uint pageCount)
        {            
            // 메모리 맵 파일을 엽니다.
            _memMap = memMap;

            // 메모리 시작 위치를 구합니다. (byte)
            long startPos;
            if(pageIndex >= pageCount)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                startPos = PageSize * pageIndex;
            }
            // 메모리 맵 뷰 클래스를 생성합니다.
            _byteData = new MMData_ByteData(_memMap, startPos, ByteDataSize, MemoryMappedFileAccess.ReadWrite);
            startPos += ByteDataSize;
            _boolData = new MMData_BoolData(_memMap, startPos, BoolDataSize, MemoryMappedFileAccess.ReadWrite);
            startPos += BoolDataSize;
            _shortData = new MMData_ShortData( _memMap, startPos, ShortDataSize, MemoryMappedFileAccess.ReadWrite );
            startPos += ShortDataSize;
            _intData = new MMData_IntData(_memMap, startPos, IntDataSize, MemoryMappedFileAccess.ReadWrite);
            startPos += IntDataSize;
            _floatData = new MMData_FloatData(_memMap, startPos, FloatDataSize, MemoryMappedFileAccess.ReadWrite);
            startPos += FloatDataSize;
            _doubleData = new MMData_DoubleData(_memMap, startPos, DoubleDataSize, MemoryMappedFileAccess.ReadWrite);
            startPos += DoubleDataSize;
            _stringData = new MMData_StringData(_memMap, startPos, StringDataSize, MemoryMappedFileAccess.ReadWrite);

        }

        /// <summary>
        /// 지정한 크기의 메모리 맵 파일을 엽니다. 만약 파일이 존재 하지 않는다면 파일을 생성합니다.
        /// </summary>
        /// <param name="fileName">메모리 맵 파일 이름</param>
        /// <param name="mapName">메모리 맵 이름</param>
        /// <param name="pageCount">페이지 개수 (메모리 맵 파일의 총 크기를 결정 한다.)</param>
        /// <returns>메모리 맵 파일 클래스</returns>
        public static MemoryMappedFile CreateMemMap(string fileName, string mapName, uint pageCount)
        {
            // 메모리 맵 파일 크기를 구합니다.
            long capacity;
            if (pageCount < 1)
            {
                capacity = PageSize;
            }
            else
            {
                capacity = PageSize * pageCount;
            }
            // 디렉토리를 확인하고 없으면 생성합니다.
            var dirName = Path.GetDirectoryName(fileName);
            if (false == Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            // 메모리 맵 파일을 엽니다.
            try
            {
                using( MemoryMappedFile.OpenExisting( mapName ) )
                {
                }
            }
            catch( System.IO.FileNotFoundException )
            {
               return MemoryMappedFile.CreateNew(fileName, capacity);
               //return MemoryMappedFile.CreateFromFile(fileName, FileMode.OpenOrCreate, mapName, capacity, MemoryMappedFileAccess.ReadWriteExecute);
            } 
            return MemoryMappedFile.OpenExisting( mapName, MemoryMappedFileRights.ReadWrite );


        }
        
        /// <summary>
        /// 바이트형 데이터에 접근합니다.
        /// </summary>
        public MMData_ByteData ByteData
        {
            get { return _byteData; }
            set { _byteData = value; }
        }
        /// <summary>
        /// 바이트형 데이터에 접근합니다.
        /// </summary>
        public MMData_BoolData BoolData
        {
            get { return _boolData; }
            set { _boolData = value; }
        }
        /// <summary>
        /// 쇼트형 데이터에 접근합니다.
        /// </summary>
        public MMData_ShortData ShortData
        {
            get { return _shortData; }
            set { _shortData = value; }
        }
        /// <summary>
        /// 인트형 데이터에 접근합니다.
        /// </summary>
        public MMData_IntData IntData
        {
            get { return _intData; }
            set { _intData = value; }
        }
        /// <summary>
        /// 플롯형 데이터에 접근합니다.
        /// </summary>
        public MMData_FloatData FloatData
        {
            get { return _floatData; }
            set { _floatData = value; }
        }
        /// <summary>
        /// 더블형 데이터에 접근합니다.
        /// </summary>
        public MMData_DoubleData DoubleData
        {
            get { return _doubleData; }
            set { _doubleData = value; }
        }
        /// <summary>
        /// 스트링형 데이터에 접근합니다.
        /// </summary>
        public MMData_StringData StringData
        {
            get { return _stringData; }
            set { _stringData = value; }
        }

        /// <summary>
        /// 입력한 데이터를 내부 데이터 클래스에 복사한다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy(MMVisionData_PageBase sourceData)
        {
            _byteData.Copy(sourceData.ByteData);
            _boolData.Copy(sourceData.BoolData);
            _shortData.Copy( sourceData.ShortData );
            _intData.Copy(sourceData.IntData);
            _floatData.Copy(sourceData.FloatData);
            _doubleData.Copy(sourceData.DoubleData);
            _stringData.Copy(sourceData.StringData);
        }
        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            _byteData.Clear();
            _boolData.Clear();
            _shortData.Clear();
            _intData.Clear();
            _floatData.Clear();
            _doubleData.Clear();
            _stringData.Clear();
        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _stringData.Dispose();
            _doubleData.Dispose();
            _floatData.Dispose();
            _shortData.Clear();
            _intData.Dispose();
            _boolData.Dispose();
            _byteData.Dispose();
            _memMap.Dispose();
        }    
    }

    #region 메모리 맵 타입별 데이터 클래스 정의
    /// <summary>
    /// 바이트형 데이터 클래스 입니다.
    /// </summary>
    sealed class MMData_ByteData : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 메모리 맵 뷰 입니다.
        /// </summary>
        private MemoryMappedViewAccessor _memView;
        /// <summary>
        /// 바이트형의 크기 입니다.
        /// </summary>
        private readonly int _typeSize = 1; // sizeof(byte) == 1
        /// <summary>
        /// 메모리 맵의 시작 위치 (바이트)
        /// </summary>
        private long _mapOffset;
        /// <summary>
        /// 메모리 맵 크기 (바이트)
        /// </summary>
        private long _mapSize;

        /// <summary>
        /// 바이트형 데이터 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="offset">메모리 맵 시작 위치 (바이트)</param>
        /// <param name="size">메모리 맵 크기 (바이트)</param>
        /// <param name="access">접근 권한</param>
        public MMData_ByteData(MemoryMappedFile memMap, long offset, long size, MemoryMappedFileAccess access)
        {
            _memMap = memMap;
            _memView = _memMap.CreateViewAccessor(offset, size, access);
            _mapOffset = offset;
            _mapSize = size;
        }

        /// <summary>
        /// 바이트형 데이터의 인덱서 입니다.
        /// </summary>
        /// <param name="idx">반환 할 데이터의 인덱스</param>
        /// <returns>인덱스에 위치한 값</returns>
        public byte this[int idx]
        {
            get
            {
                var position = getPosition(idx);
                byte readValue;
                _memView.Read(position, out readValue);
                return readValue;
            }
            set
            {
                var position = getPosition(idx);
                _memView.Write(position, value);
            }
        }

        /// <summary>
        /// 인덱스에 해당하는 메모리 위치(바이트)를 계산합니다.
        /// </summary>
        /// <param name="idx">데이터의 인덱스</param>
        /// <returns>메모리 위치(바이트)</returns>
        private int getPosition(int idx)
        {
            var targetPosition = _typeSize * idx;
            if ((targetPosition + _typeSize) >= _memView.Capacity)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return targetPosition;
            }
        }

        /// <summary>
        /// 입력한 데이터를 복사합니다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy(MMData_ByteData sourceData)
        {
            var copySourceData = sourceData.ToBytes();
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        public void Copy( byte[] sourceData )
        {
            var copySourceData = sourceData;
            _memView.WriteArray( 0, copySourceData, 0, ( int )sourceData.Length );
        }


        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            var copySourceData = new byte[_mapSize];
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 전체 데이터의 바이트 배열을 읽어 옵니다.
        /// </summary>
        /// <returns>내부 전체 데이터의 배열</returns>
        public byte[] ToBytes()
        {
            var data = new byte[_mapSize];
            _memView.ReadArray(0, data, 0, (int)_mapSize);
            return data;
        }

        public byte[] ToBytes( int iReadSize )
        {
            var data = new byte[ _mapSize ];
            _memView.ReadArray( 0, data, 0, iReadSize );
            return data;

        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _memView.Dispose();
        }
    }

    /// <summary>
    /// 불형 데이터 클래스 입니다.
    /// </summary>
    sealed class MMData_BoolData : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 메모리 맵 뷰 입니다.
        /// </summary>
        private MemoryMappedViewAccessor _memView;
        /// <summary>
        /// 바이트형의 크기 입니다.
        /// </summary>
        private readonly int _typeSize = 1; // sizeof(bool) == 1
        /// <summary>
        /// 메모리 맵의 시작 위치 (바이트)
        /// </summary>
        private long _mapOffset;
        /// <summary>
        /// 메모리 맵 크기 (바이트)
        /// </summary>
        private long _mapSize;

        /// <summary>
        /// 바이트형 데이터 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="offset">메모리 맵 시작 위치 (바이트)</param>
        /// <param name="size">메모리 맵 크기 (바이트)</param>
        /// <param name="access">접근 권한</param>
        public MMData_BoolData(MemoryMappedFile memMap, long offset, long size, MemoryMappedFileAccess access)
        {
            _memMap = memMap;
            _memView = _memMap.CreateViewAccessor(offset, size, access);
            _mapOffset = offset;
            _mapSize = size;
        }

        /// <summary>
        /// 바이트형 데이터의 인덱서 입니다.
        /// </summary>
        /// <param name="idx">반환 할 데이터의 인덱스</param>
        /// <returns>인덱스에 위치한 값</returns>
        public bool this[int idx]
        {
            get
            {
                var position = getPosition(idx);
                bool readValue;
                _memView.Read(position, out readValue);
                return readValue;
            }
            set
            {
                var position = getPosition(idx);
                _memView.Write(position, value);
            }
        }

        /// <summary>
        /// 인덱스에 해당하는 메모리 위치(바이트)를 계산합니다.
        /// </summary>
        /// <param name="idx">데이터의 인덱스</param>
        /// <returns>메모리 위치(바이트)</returns>
        private int getPosition(int idx)
        {
            var targetPosition = _typeSize * idx;
            if ((targetPosition + _typeSize) >= _memView.Capacity)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return targetPosition;
            }
        }

        /// <summary>
        /// 입력한 데이터를 복사합니다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy(MMData_BoolData sourceData)
        {
            var copySourceData = sourceData.ToBytes();
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            var copySourceData = new byte[_mapSize];
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 전체 데이터의 바이트 배열을 읽어 옵니다.
        /// </summary>
        /// <returns>내부 전체 데이터의 배열</returns>
        public byte[] ToBytes()
        {
            var data = new byte[_mapSize];
            _memView.ReadArray(0, data, 0, (int)_mapSize);
            return data;
        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _memView.Dispose();
        }
    }

    /// <summary>
    /// 쇼트형 데이터 클래스 입니다.
    /// </summary>
    sealed class MMData_ShortData : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 메모리 맵 뷰 입니다.
        /// </summary>
        private MemoryMappedViewAccessor _memView;
        /// <summary>
        /// 인트형의 크기 입니다.
        /// </summary>
        private readonly short _typeSize = 2; // sizeof(short) == 2
        /// <summary>
        /// 메모리 맵의 시작 위치 (바이트)
        /// </summary>
        private long _mapOffset;
        /// <summary>
        /// 메모리 맵 크기 (바이트)
        /// </summary>
        private long _mapSize;

        /// <summary>
        /// 인트형 데이터 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="offset">메모리 맵 시작 위치 (바이트)</param>
        /// <param name="size">메모리 맵 크기 (바이트)</param>
        /// <param name="access">접근 권한</param>
        public MMData_ShortData( MemoryMappedFile memMap, long offset, long size, MemoryMappedFileAccess access )
        {
            _memMap = memMap;
            _memView = _memMap.CreateViewAccessor( offset, size, access );
            _mapOffset = offset;
            _mapSize = size;
        }

        /// <summary>
        /// 인트형 데이터의 인덱서 입니다.
        /// </summary>
        /// <param name="idx">반환 할 데이터의 인덱스</param>
        /// <returns>인덱스에 위치한 값</returns>
        public short this[ int idx ]
        {
            get
            {
                var position = getPosition( idx );
                short readValue;
                _memView.Read( position, out readValue );
                return readValue;
            }
            set
            {
                var position = getPosition( idx );
                _memView.Write( position, value );
            }
        }

        /// <summary>
        /// 인덱스에 해당하는 메모리 위치(바이트)를 계산합니다.
        /// </summary>
        /// <param name="idx">데이터의 인덱스</param>
        /// <returns>메모리 위치(바이트)</returns>
        private int getPosition( int idx )
        {
            var targetPosition = _typeSize * idx;
            if( ( targetPosition + _typeSize ) >= _memView.Capacity )
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return targetPosition;
            }
        }

        /// <summary>
        /// 입력한 데이터를 복사합니다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy( MMData_ShortData sourceData )
        {
            var copySourceData = sourceData.ToBytes();
            _memView.WriteArray( 0, copySourceData, 0, ( short )_mapSize );
        }

        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            var copySourceData = new byte[ _mapSize ];
            _memView.WriteArray( 0, copySourceData, 0, ( short )_mapSize );
        }

        /// <summary>
        /// 내부 전체 데이터의 바이트 배열을 읽어 옵니다.
        /// </summary>
        /// <returns>내부 전체 데이터의 배열</returns>
        public byte[] ToBytes()
        {
            var data = new byte[ _mapSize ];
            _memView.ReadArray( 0, data, 0, ( short )_mapSize );
            return data;
        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _memView.Dispose();
        }
    }


    /// <summary>
    /// 인트형 데이터 클래스 입니다.
    /// </summary>
    sealed class MMData_IntData : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 메모리 맵 뷰 입니다.
        /// </summary>
        private MemoryMappedViewAccessor _memView;
        /// <summary>
        /// 인트형의 크기 입니다.
        /// </summary>
        private readonly int _typeSize = 4; // sizeof(int) == 4
        /// <summary>
        /// 메모리 맵의 시작 위치 (바이트)
        /// </summary>
        private long _mapOffset;
        /// <summary>
        /// 메모리 맵 크기 (바이트)
        /// </summary>
        private long _mapSize;

        /// <summary>
        /// 인트형 데이터 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="offset">메모리 맵 시작 위치 (바이트)</param>
        /// <param name="size">메모리 맵 크기 (바이트)</param>
        /// <param name="access">접근 권한</param>
        public MMData_IntData(MemoryMappedFile memMap, long offset, long size, MemoryMappedFileAccess access)
        {
            _memMap = memMap;
            _memView = _memMap.CreateViewAccessor(offset, size, access);
            _mapOffset = offset;
            _mapSize = size;
        }

        /// <summary>
        /// 인트형 데이터의 인덱서 입니다.
        /// </summary>
        /// <param name="idx">반환 할 데이터의 인덱스</param>
        /// <returns>인덱스에 위치한 값</returns>
        public int this[int idx]
        {
            get
            {
                var position = getPosition(idx);
                int readValue;
                _memView.Read(position, out readValue);
                return readValue;
            }
            set
            {
                var position = getPosition(idx);
                _memView.Write(position, value);
            }
        }

        /// <summary>
        /// 인덱스에 해당하는 메모리 위치(바이트)를 계산합니다.
        /// </summary>
        /// <param name="idx">데이터의 인덱스</param>
        /// <returns>메모리 위치(바이트)</returns>
        private int getPosition(int idx)
        {
            var targetPosition = _typeSize * idx;
            if((targetPosition + _typeSize) >= _memView.Capacity)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return targetPosition;
            }
        }

        /// <summary>
        /// 입력한 데이터를 복사합니다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy(MMData_IntData sourceData)
        {
            var copySourceData = sourceData.ToBytes();
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            var copySourceData = new byte[_mapSize];
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 전체 데이터의 바이트 배열을 읽어 옵니다.
        /// </summary>
        /// <returns>내부 전체 데이터의 배열</returns>
        public byte[] ToBytes()
        {
            var data = new byte[_mapSize];
            _memView.ReadArray(0, data, 0, (int)_mapSize);
            return data;
        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _memView.Dispose();
        }
    }

    /// <summary>
    /// 플롯형 데이터 클래스 입니다.
    /// </summary>
    sealed class MMData_FloatData : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 메모리 맵 뷰 입니다.
        /// </summary>
        private MemoryMappedViewAccessor _memView;
        /// <summary>
        /// 플롯형의 크기 입니다.
        /// </summary>
        private readonly int _typeSize = 4; // sizeof(float) == 4
        /// <summary>
        /// 메모리 맵의 시작 위치 (바이트)
        /// </summary>
        private long _mapOffset;
        /// <summary>
        /// 메모리 맵 크기 (바이트)
        /// </summary>
        private long _mapSize;

        /// <summary>
        /// 플롯형 데이터 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="offset">메모리 맵 시작 위치 (바이트)</param>
        /// <param name="size">메모리 맵 크기 (바이트)</param>
        /// <param name="access">접근 권한</param>
        public MMData_FloatData(MemoryMappedFile memMap, long offset, long size, MemoryMappedFileAccess access)
        {
            _memMap = memMap;
            _memView = _memMap.CreateViewAccessor(offset, size, access);
            _mapOffset = offset;
            _mapSize = size;
        }

        /// <summary>
        /// 플롯형 데이터의 인덱서 입니다.
        /// </summary>
        /// <param name="idx">반환 할 데이터의 인덱스</param>
        /// <returns>인덱스에 위치한 값</returns>
        public float this[int idx]
        {
            get
            {
                var position = getPosition(idx);
                float readValue;
                _memView.Read(position, out readValue);
                return readValue;
            }
            set
            {
                var position = getPosition(idx);
                _memView.Write(position, value);
            }
        }

        /// <summary>
        /// 인덱스에 해당하는 메모리 위치(바이트)를 계산합니다.
        /// </summary>
        /// <param name="idx">데이터의 인덱스</param>
        /// <returns>메모리 위치(바이트)</returns>
        private int getPosition(int idx)
        {
            var targetPosition = _typeSize * idx;
            if ((targetPosition + _typeSize) >= _memView.Capacity)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return targetPosition;
            }
        }

        /// <summary>
        /// 입력한 데이터를 복사합니다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy(MMData_FloatData sourceData)
        {
            var copySourceData = sourceData.ToBytes();
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            var copySourceData = new byte[_mapSize];
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 전체 데이터의 바이트 배열을 읽어 옵니다.
        /// </summary>
        /// <returns>내부 전체 데이터의 배열</returns>
        public byte[] ToBytes()
        {
            var data = new byte[_mapSize];
            _memView.ReadArray(0, data, 0, (int)_mapSize);
            return data;
        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _memView.Dispose();
        }
    }

    /// <summary>
    /// 더블형 데이터 클래스 입니다.
    /// </summary>
    sealed class MMData_DoubleData : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 메모리 맵 뷰 입니다.
        /// </summary>
        private MemoryMappedViewAccessor _memView;
        /// <summary>
        /// 플롯형의 크기 입니다.
        /// </summary>
        private readonly int _typeSize = 8; // sizeof(double) == 8
        /// <summary>
        /// 메모리 맵의 시작 위치 (바이트)
        /// </summary>
        private long _mapOffset;
        /// <summary>
        /// 메모리 맵 크기 (바이트)
        /// </summary>
        private long _mapSize;

        /// <summary>
        /// 플롯형 데이터 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="offset">메모리 맵 시작 위치 (바이트)</param>
        /// <param name="size">메모리 맵 크기 (바이트)</param>
        /// <param name="access">접근 권한</param>
        public MMData_DoubleData(MemoryMappedFile memMap, long offset, long size, MemoryMappedFileAccess access)
        {
            _memMap = memMap;
            _memView = _memMap.CreateViewAccessor(offset, size, access);
            _mapOffset = offset;
            _mapSize = size;
        }

        /// <summary>
        /// 플롯형 데이터의 인덱서 입니다.
        /// </summary>
        /// <param name="idx">반환 할 데이터의 인덱스</param>
        /// <returns>인덱스에 위치한 값</returns>
        public double this[int idx]
        {
            get
            {
                var position = getPosition(idx);
                double readValue;
                _memView.Read(position, out readValue);
                return readValue;
            }
            set
            {
                var position = getPosition(idx);
                _memView.Write(position, value);
            }
        }

        /// <summary>
        /// 인덱스에 해당하는 메모리 위치(바이트)를 계산합니다.
        /// </summary>
        /// <param name="idx">데이터의 인덱스</param>
        /// <returns>메모리 위치(바이트)</returns>
        private int getPosition(int idx)
        {
            var targetPosition = _typeSize * idx;
            if ((targetPosition + _typeSize) >= _memView.Capacity)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return targetPosition;
            }
        }

        /// <summary>
        /// 입력한 데이터를 복사합니다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy(MMData_DoubleData sourceData)
        {
            var copySourceData = sourceData.ToBytes();
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            var copySourceData = new byte[_mapSize];
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 전체 데이터의 바이트 배열을 읽어 옵니다.
        /// </summary>
        /// <returns>내부 전체 데이터의 배열</returns>
        public byte[] ToBytes()
        {
            var data = new byte[_mapSize];
            _memView.ReadArray(0, data, 0, (int)_mapSize);
            return data;
        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _memView.Dispose();
        }
    }

    /// <summary>
    /// 스트링형 데이터 클래스 입니다.
    /// </summary>
    sealed class MMData_StringData : IDisposable
    {
        /// <summary>
        /// 메모리 맵 파일 입니다.
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 메모리 맵 뷰 입니다.
        /// </summary>
        private MemoryMappedViewAccessor _memView;
        /// <summary>
        /// 스트링형의 크기 입니다.
        /// </summary>
        private readonly int _typeSize = 1; // sizeof(byte) == 1
        /// <summary>
        /// 문자열의 최대 길이 입니다.
        /// </summary>
        private readonly int _stringLength = 512;
        /// <summary>
        /// 메모리 맵의 시작 위치 (바이트)
        /// </summary>
        private long _mapOffset;
        /// <summary>
        /// 메모리 맵 크기 (바이트)
        /// </summary>
        private long _mapSize;

        /// <summary>
        /// 스트링형 데이터 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="offset">메모리 맵 시작 위치 (바이트)</param>
        /// <param name="size">메모리 맵 크기 (바이트)</param>
        /// <param name="access">접근 권한</param>
        public MMData_StringData(MemoryMappedFile memMap, long offset, long size, MemoryMappedFileAccess access)
        {
            _memMap = memMap;
            _memView = _memMap.CreateViewAccessor(offset, size, access);
            _mapOffset = offset;
            _mapSize = size;
        }

        /// <summary>
        /// 스트링형 데이터의 인덱서 입니다.
        /// </summary>
        /// <param name="idx">반환 할 데이터의 인덱스</param>
        /// <returns>인덱스에 위치한 값</returns>
        public string this[int idx]
        {
            get
            {
                var position = getPosition(idx);

                var buffer = new byte[_stringLength];

                _memView.ReadArray(position, buffer, 0, _stringLength);

                return Encoding.UTF8.GetString(buffer).Trim('\0');
            }
            set
            {
                var position = getPosition(idx);

                var clearBuffer = new byte[_stringLength];
                _memView.WriteArray(position, clearBuffer, 0, clearBuffer.Count());

                var buffer = Encoding.UTF8.GetBytes(value);
                _memView.WriteArray(position, buffer, 0, buffer.Count());
            }
        }

        /// <summary>
        /// 인덱스에 해당하는 메모리 위치(바이트)를 계산합니다.
        /// </summary>
        /// <param name="idx">데이터의 인덱스</param>
        /// <returns>메모리 위치(바이트)</returns>
        private int getPosition(int idx)
        {
            var targetPosition = _typeSize * _stringLength * idx;
            if ((targetPosition + (_typeSize * _stringLength)) >= _memView.Capacity)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return targetPosition;
            }
        }

        /// <summary>
        /// 입력한 데이터를 복사합니다.
        /// </summary>
        /// <param name="sourceData">복사 할 데이터</param>
        public void Copy(MMData_StringData sourceData)
        {
            var copySourceData = sourceData.ToBytes();
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 데이터를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            var copySourceData = new byte[_mapSize];
            _memView.WriteArray(0, copySourceData, 0, (int)_mapSize);
        }

        /// <summary>
        /// 내부 전체 데이터의 바이트 배열을 읽어 옵니다.
        /// </summary>
        /// <returns>내부 전체 데이터의 배열</returns>
        public byte[] ToBytes()
        {
            var data = new byte[_mapSize];
            _memView.ReadArray(0, data, 0, (int)_mapSize);
            return data;
        }

        /// <summary>
        /// Dispose()를 호출하면 메모리를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _memView.Dispose();
        }
    }
    #endregion
}