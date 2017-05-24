using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Xone;

namespace RepositoryImplement.Xone.RepositoryDerive
{
   public interface IRaceMaster : IDisposable
    {
        IEnumerable<TblRace> ListRaceDetails();
        int CreateRace(TblRace RaceObj);
    }



}