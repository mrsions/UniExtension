#nullable enable

using System.Collections.Generic;
using UniExtension.Collections;

public static class BadSample
{
    public static bool disable { get; set; }
    public static PList<int>? staticVar { get; set; } // 경고: 풀링용 객체는 변수로 저장하지 않아야합니다. 풀링용이 아닌 객체로 생성하세요.

    public static void LeakList()
    {
        var list = PList.Take<int>(); // UEA0002:
        list.Add(1);
    }

    public static void NotDisposedList()
    {
        var list = PList.Take<int>(); // 경고: Dispose가 호출되지 않고 종료될 수 있습니다.
        list.Add(1);
        if (disable)
        {
            list.Dispose();
        }
    }

    public static void FinallyNotUsed()
    {
        PList<int>? list = null;
        try
        {
            list = PList.Take<int>(); // UEA0002:
            list.Add(1);
        }
        finally
        {
            //not disposed
        }
    }

    public static void PutVar()
    {
        PList<int>? list = null;
        list = PList.Take<int>(); // 경고: 사용후 dispose 하세요
        list.Add(1);
    }

    public static void DoublePutVar()
    {
        PList<int>? list = null;
        list = PList.Take<int>();
        list = PList.Take<int>(); // 경고: 재할당 전 dispose하세요
    }

    public static void StoreVar()
    {
        staticVar = PList.Take<int>(); // 경고: 맴버에 저장하지마세요
    }

    public static List<int> CreateList()
    {
        var list = PList.Take<int>();
        list.Add(1);
        return list; // UEA0001: 
    }
}
