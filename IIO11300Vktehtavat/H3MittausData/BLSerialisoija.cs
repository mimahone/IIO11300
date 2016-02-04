using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace JAMK.IT.IIO11300
{
  public class Serialisointi
  {
    #region XmlTiedostoMetodit

    public static void SerialisoiXml(string tiedosto, ICollection ic)
    {
      XmlSerializer xs = new XmlSerializer(ic.GetType());
      TextWriter tw = new StreamWriter(tiedosto);

      try
      {
        xs.Serialize(tw, ic);
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        tw.Close();
      }
    }

    public static List<MittausData> DeSerialisoiXml(string tiedosto)
    {
      //deserialisointi, huom vain tyypitetylle listalle MittausData-olioita!!
      XmlSerializer deserializer = new XmlSerializer(typeof(List<MittausData>));
      TextReader tr = new StreamReader(tiedosto);

      try
      {
        return (List<MittausData>)deserializer.Deserialize(tr);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        tr.Close();
      }
    }

    #endregion

    #region BinääriTiedostoMetodit

    public static void Serialisoi(string tiedosto, ICollection ic)
    {
      //serialisoidaan Collection
      FileStream fs = new FileStream(tiedosto, FileMode.Create);
      BinaryFormatter bf = new BinaryFormatter();

      try
      {
        bf.Serialize(fs, ic);
      }
      catch (SerializationException se)
      {
        throw se;
      }
      finally
      {
        fs.Close();
      }
    }

    public static void DeSerialisoi(string tiedosto, ref object objekti)
    {
      //deserialisointi
      FileStream fs = new FileStream(tiedosto, FileMode.Open);

      try
      {
        BinaryFormatter bf = new BinaryFormatter();
        objekti = bf.Deserialize(fs);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        fs.Close();
      }
    }

    #endregion
  }
}
