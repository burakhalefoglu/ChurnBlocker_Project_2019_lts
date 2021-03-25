﻿using Assets.Appneuron.Core.CoreServices.SaveDataServices.Abstract;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess
{
    public interface IGameSessionEveryLoginDal : IRepositoryService<GameSessionEveryLoginDataModel>
    {
    }
}
