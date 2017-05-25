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
        TblRace GetDetailsForEdit(Int16 RaceID);
        int EditRaceDetails(TblRace RaceObj);
        bool CheckRaceDeletableStatus(Int16 RaceID);
        string GetRaceName(Int16 RaceID);
        int DeleteRace(Int16 RaceID);

    }


     
}