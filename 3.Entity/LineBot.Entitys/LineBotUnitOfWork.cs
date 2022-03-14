using LineBot.Entitys.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using URF.Core.EF;

namespace LineBot.Entitys
{
    public class LineBotUnitOfWork : UnitOfWork, ILineBotUnitOfWork, IUnitOfWork
    {
        public LineBotUnitOfWork(LineBotContext context) : base(context)
        {
        }
    }
}
