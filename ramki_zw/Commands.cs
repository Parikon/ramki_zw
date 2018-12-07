using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using zzr = ZwSoft.ZwCAD.Runtime;
using zza = ZwSoft.ZwCAD.ApplicationServices;

namespace ramki_zw
{
    public class Commands
    {
        [zzr.CommandMethod("PI_ramki")]

        public void otworzokno()
        {
            Okno_ramki win = new Okno_ramki(445, 380);
            zza.Application.ShowModalWindow(win);
        }

    }
}
