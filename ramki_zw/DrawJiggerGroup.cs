using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using zze = ZwSoft.ZwCAD.EditorInput;
using zzg = ZwSoft.ZwCAD.Geometry;
using zzd = ZwSoft.ZwCAD.DatabaseServices;
using zzgr = ZwSoft.ZwCAD.GraphicsInterface;
using zza = ZwSoft.ZwCAD.ApplicationServices;

namespace ramki_zw
{
    
    public partial class DrawJiggerGroup : zze.DrawJig
    {
        // na podstawie przykładów z https://spiderinnet1.typepad.com/blog/
        #region Fields

        private string zapytanie = "\nWskaż lewy dolny róg ramki:";
        private zzg.Point3d mBase;
        private zzg.Point3d mLocation;
        List<zzd.Entity> mEntities;

        #endregion

        #region Constructors

        public DrawJiggerGroup(zzg.Point3d basePt)
        {
            mBase = basePt.TransformBy(UCS);
            mEntities = new List<zzd.Entity>();
        }

        #endregion

        #region Properties

        public zzg.Point3d Base
        {
            get { return mLocation; }
            set { mLocation = value; }
        }

        public zzg.Point3d Location
        {
            get { return mLocation; }
            set { mLocation = value; }
        }

        public zzg.Matrix3d UCS
        {
            get
            {
                return zza.Application.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;
            }
        }

        #endregion

        #region Methods

        public void AddEntity(zzd.Entity ent)
        {
            mEntities.Add(ent);
        }

        public void TransformEntities()
        {
            zzg.Matrix3d mat = zzg.Matrix3d.Displacement(mBase.GetVectorTo(mLocation));

            foreach (zzd.Entity ent in mEntities)
            {
                ent.TransformBy(mat);
            }
        }
        #endregion

        #region Overrides

        protected override bool WorldDraw(zzgr.WorldDraw draw)
        {
            zzg.Matrix3d mat = zzg.Matrix3d.Displacement(mBase.GetVectorTo(mLocation));

            zzgr.WorldGeometry geo = draw.Geometry;
            if (geo != null)
            {
                geo.PushModelTransform(mat);

                foreach (zzd.Entity ent in mEntities)
                {
                    geo.Draw(ent);
                }

                geo.PopModelTransform();
            }

            return true;
        }

        protected override zze.SamplerStatus Sampler(zze.JigPrompts prompts)
        {
            
                zze.JigPromptPointOptions prOptions1 = new zze.JigPromptPointOptions(zapytanie);
                prOptions1.UseBasePoint = false;

                zze.PromptPointResult prResult1 = prompts.AcquirePoint(prOptions1);
                if (prResult1.Status == zze.PromptStatus.Cancel || prResult1.Status == zze.PromptStatus.Error)
                    return zze.SamplerStatus.Cancel;

                if (!mLocation.IsEqualTo(prResult1.Value, new zzg.Tolerance(10e-10, 10e-10)))
                {
                    mLocation = prResult1.Value;
                    return zze.SamplerStatus.OK;
                }
                else
                return zze.SamplerStatus.NoChange;              
            
        }

        #endregion


    }
}
