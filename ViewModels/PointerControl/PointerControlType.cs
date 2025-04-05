using DirN.Utils.Nodes.Attributes;
using DirN.Views.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public enum PointerControlType
    {
        None,

        [HPCSet(typeof(PString),typeof(PStringViewModel))]
        PString,

        [HPCSet(typeof(PInt), typeof(PIntViewModel))]
        PInt,

        [HPCSet(typeof(PAny), typeof(PAnyViewModel))]
        PAny,

        [HPCSet(typeof(PFileInfo), typeof(PFileInfoViewModel))]
        PFileInfo,

        [HPCSet(typeof(PEnum), typeof(PEnumViewModel))]
        PEnum,

        [HPCSet(typeof(PBool), typeof(PBoolViewModel))]
        PBool,
    }
}
