using prolab_THEEND.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;

namespace prolab_THEEND.Commands
{
    public static class LinqByMe<T> where T : class
    {
        //public static object FirstOrNull(List<T> list, object find)
        //{
        //    foreach (var item in list)
        //    {
        //        if (item is Hasta devran)
        //        {
        //            devran.Ad;    // hangisi sorgulanacak ? parametre olarak 'Property' türü yollanamadığı için burada kafam karıştı.
        //            devran.SoyAd; // çünkü biz parametre olarak cshtml'de hasta.Ad olarak yolluyoruz, ama tam olarak bu kod kısmında
        //        }                 // fonksiyonun hangi 'Property (ad , soyad , vb.) 'yi sorguladığını belirlemek imkansız. 
        //    }// Yapmak tabii mümkün her property için ayrı fonksiyon yazar ve o bilinçte parametre yollarsak ama bu da çok performanssız.
        //    return null;
        //}
        //public static List<T> WhereEquation(List<T> list ,object lookingFor)
        //{
        //    List<T> tempList = new List<T>();
        //    foreach (var item in list)
        //        if (lookingFor == item)
        //        {
        //            tempList.Add(item);
        //            return tempList;
        //        }
        //    return null;
        //}
    }
    //public static class LinqByMeComparise<T,G> where T : class,IComparable<T> where G : IComparable<G>
    //{
    //    public static List<G> WhereCompareNumerable(List<T> list, G sendedValue, bool doesCheckBigger, bool isEqualityIncluded)
    //    {
    //        List<G> tempList = new List<G>();

    //        if (doesCheckBigger)  // Where(x=>x.id > kullanıcı.id)   
    //        {
    //            foreach (var item in list)
    //            {
    //                if (isEqualityIncluded)
    //                    if (sendedValue.CompareTo(item as G) >= 0)
    //                        tempList.Add(item);
    //                    else { }
    //                else
    //                    if (sendedValue.CompareTo(item) > 0)
    //                        tempList.Add(item);
    //            }
    //        }
    //        else
    //        {
    //            foreach (var item in list)
    //            {
    //                if (isEqualityIncluded)
    //                    if (sendedValue.CompareTo(item) <= 0)
    //                        tempList.Add(item);
    //                    else { }
    //                else
    //                    if (sendedValue.CompareTo(item) < 0)
    //                    tempList.Add(item);
    //            }
    //        }
    //        return tempList;

    //    }
    //}
}