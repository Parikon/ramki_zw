using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using zze = ZwSoft.ZwCAD.EditorInput;
using zzg = ZwSoft.ZwCAD.Geometry;
using zzd = ZwSoft.ZwCAD.DatabaseServices;
using zzgr = ZwSoft.ZwCAD.GraphicsInterface;
using zza = ZwSoft.ZwCAD.ApplicationServices;
using zzc = ZwSoft.ZwCAD.Colors;

namespace ramki_zw
{
    public class Ramka
    {
        /// <summary>
        /// Wstawia warstwę o podanej nazwie, kolorze i czyni ją plotowaną lub nie.
        /// </summary>
        /// <param name="layername"></param>
        /// <param name="color"></param>
        /// <param name="isplotable"></param>
        public static void WstawWarstwe(string layername, short color, bool isplotable)
        {
            zza.Document doc = zza.Application.DocumentManager.MdiActiveDocument;
            zzd.Database db = doc.Database;
            using (zzd.Transaction tr = db.TransactionManager.StartTransaction())
            {

                zzd.LayerTable lt = (zzd.LayerTable)tr.GetObject(db.LayerTableId, zzd.OpenMode.ForRead);
                if (lt.Has(layername) == false)
                {
                    // Tworzymy nową wartwę papier
                    zzd.LayerTableRecord nowawarstwa = new zzd.LayerTableRecord();
                    //nadajemy jej wlasciwosci
                    nowawarstwa.Name = layername;
                    nowawarstwa.IsPlottable = isplotable;
                    nowawarstwa.Color = zzc.Color.FromColorIndex(zzc.ColorMethod.ByAci, color);
                    //dadaj nowarstwa do tabeli
                    lt.UpgradeOpen();
                    zzd.ObjectId warstwa = lt.Add(nowawarstwa);
                    tr.AddNewlyCreatedDBObject(nowawarstwa, true);
                }

                tr.Commit();
            }
        }

        /// <summary>
        /// Sprawdza czy jesteśmy w przestrzenii modelu
        /// </summary>
        /// <returns></returns>        
        public static bool ItisModel()
        {
            if (zza.Application.DocumentManager.MdiActiveDocument.Database.TileMode)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Sprawdza czy w przestrzeni papieru jesteśmy  poza oknem true lub w oknie false
        /// na podstawie https://spiderinnet1.typepad.com/blog/2014/05/autocad-net-detect-current-space-model-or-paper-and-viewport.html
        /// </summary>
        /// <returns></returns>
        public static bool IsInLayoutPaper()
        {
            zza.Document doc = zza.Application.DocumentManager.MdiActiveDocument;
            zzd.Database db = doc.Database;
            zze.Editor ed = doc.Editor;

            if (db.TileMode)
                return false;
            else
            {
                if (db.PaperSpaceVportId == zzd.ObjectId.Null)
                    return false;
                else if (ed.CurrentViewportObjectId == zzd.ObjectId.Null)
                    return false;
                else if (ed.CurrentViewportObjectId == db.PaperSpaceVportId)
                    return true;
                else
                    return false;
            }
        }

        public static void Rysuj_ramke()
        {
            for (; ; )
            {

                int wysokosc = UserControl1.wysokosc;
                int szerokosc = UserControl1.dlugosc;
                int margines_gorny = UserControl1.G_marg;
                int margines_dolny = UserControl1.D_marg;
                int margines_lewy = UserControl1.L_marg;
                int margines_prawy = UserControl1.P_marg;

                string papier = "PI_PAPIER";
                string ramka = "PI_RAMKA";

                WstawWarstwe(papier, 7, false);
                WstawWarstwe(ramka, 2, true);

                //pobierz aktualny rysunek i database
                zza.Document doc = zza.Application.DocumentManager.MdiActiveDocument;
                zzd.Database db = doc.Database;
                zze.Editor ed = doc.Editor;                

                bool jestmodel = ItisModel();
                bool jestviewport = IsInLayoutPaper();

                int tilemode = Convert.ToInt16(zza.Application.GetSystemVariable("TILEMODE"));

                zzg.Point3d ptStart = new zzg.Point3d(-100000, -100000, 0);
                //zzg.Point3d ptStart = pPtRes.Value;

                DrawJiggerGroup jigger2 = new DrawJiggerGroup(ptStart);

                using (zzd.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    //utworzenie grupy
                    zzd.DBDictionary groupDic = (zzd.DBDictionary)tr.GetObject(db.GroupDictionaryId, zzd.OpenMode.ForWrite);
                    zzd.Group anonyGroup = new zzd.Group();
                    groupDic.SetAt("*", anonyGroup);

                    zzd.BlockTable bt;
                    zzd.BlockTableRecord btr;

                    // Open Model space for write
                    bt = tr.GetObject(db.BlockTableId,
                                                    zzd.OpenMode.ForRead) as zzd.BlockTable;
                    if (jestmodel == false & jestviewport == true)
                    {
                        btr = tr.GetObject(bt[zzd.BlockTableRecord.PaperSpace],
                                   zzd.OpenMode.ForWrite) as zzd.BlockTableRecord;
                    }
                    else
                    {
                        btr = tr.GetObject(bt[zzd.BlockTableRecord.ModelSpace],
                                                   zzd.OpenMode.ForWrite) as zzd.BlockTableRecord;
                    }

                    if (wysokosc != 210)
                    {
                        // Create a papier polyline 
                        using (zzd.Polyline acPoly = new zzd.Polyline())
                        {
                            zzg.Point2d p1 = new zzg.Point2d(ptStart.X, ptStart.Y + margines_dolny);
                            zzg.Point2d p2 = new zzg.Point2d(ptStart.X, ptStart.Y);
                            zzg.Point2d p3 = new zzg.Point2d(ptStart.X + szerokosc, ptStart.Y);
                            zzg.Point2d p4 = new zzg.Point2d(ptStart.X + szerokosc, ptStart.Y + wysokosc);
                            zzg.Point2d p5 = new zzg.Point2d(ptStart.X, ptStart.Y + wysokosc);
                            zzg.Point2d p6 = new zzg.Point2d(ptStart.X, ptStart.Y + 297);

                            acPoly.AddVertexAt(0, p1, 0, 0, 0);
                            acPoly.AddVertexAt(1, p2, 0, 0, 0);
                            acPoly.AddVertexAt(2, p3, 0, 0, 0);
                            acPoly.AddVertexAt(3, p4, 0, 0, 0);
                            acPoly.AddVertexAt(4, p5, 0, 0, 0);
                            acPoly.AddVertexAt(5, p6, 0, 0, 0);

                            //acPoly.Closed = true;
                            //acPoly.ColorIndex = kolorpapier;
                            acPoly.Layer = papier;

                            // Add the new object to the block table record and the transaction
                            btr.AppendEntity(acPoly);
                            tr.AddNewlyCreatedDBObject(acPoly, true);
                            anonyGroup.Append(acPoly.ObjectId);
                        }

                        // Create a  ramka polyline 
                        using (zzd.Polyline ramkaPoly = new zzd.Polyline())
                        {
                            zzg.Point2d r1 = new zzg.Point2d(ptStart.X + margines_lewy, ptStart.Y + margines_dolny);
                            zzg.Point2d r2 = new zzg.Point2d(ptStart.X, ptStart.Y + margines_dolny);
                            zzg.Point2d r3 = new zzg.Point2d(ptStart.X, ptStart.Y + 297);
                            zzg.Point2d r4 = new zzg.Point2d(ptStart.X + margines_lewy, ptStart.Y + 297);
                            zzg.Point2d r5 = new zzg.Point2d(ptStart.X + margines_lewy, ptStart.Y + wysokosc - margines_gorny);
                            zzg.Point2d r6 = new zzg.Point2d(ptStart.X + szerokosc - margines_prawy, ptStart.Y + wysokosc - margines_gorny);
                            zzg.Point2d r7 = new zzg.Point2d(ptStart.X + szerokosc - margines_prawy, ptStart.Y + margines_dolny);


                            ramkaPoly.AddVertexAt(0, r2, 0, 0, 0);
                            ramkaPoly.AddVertexAt(1, r3, 0, 0, 0);
                            ramkaPoly.AddVertexAt(2, r4, 0, 0, 0);
                            ramkaPoly.AddVertexAt(3, r5, 0, 0, 0);
                            ramkaPoly.AddVertexAt(4, r6, 0, 0, 0);
                            ramkaPoly.AddVertexAt(5, r7, 0, 0, 0);
                            ramkaPoly.AddVertexAt(6, r1, 0, 0, 0);
                            ramkaPoly.AddVertexAt(7, r4, 0, 0, 0);

                            //ramkaPoly.ColorIndex = kolorramki;
                            ramkaPoly.Layer = ramka;

                            // Add the new object to the block table record and the transaction
                            btr.AppendEntity(ramkaPoly);
                            tr.AddNewlyCreatedDBObject(ramkaPoly, true);
                            anonyGroup.Append(ramkaPoly.ObjectId);
                        }

                        //tworzę miejsca na dziurkacz
                        using (zzd.Circle dziurka1 = new zzd.Circle())
                        {
                            dziurka1.Center = new zzg.Point3d(ptStart.X + 10.0, ptStart.Y + (297.0 / 2 - 40.0), ptStart.Z);
                            dziurka1.Radius = 2.5;
                            dziurka1.Layer = ramka;
                            //dziurka1.ColorIndex = kolorramki;
                            btr.AppendEntity(dziurka1);
                            tr.AddNewlyCreatedDBObject(dziurka1, true);
                            anonyGroup.Append(dziurka1.ObjectId);
                        }
                        using (zzd.Circle dziurka2 = new zzd.Circle())
                        {
                            dziurka2.Center = new zzg.Point3d(ptStart.X + 10.0, ptStart.Y + (297.0 / 2 + 40.0), ptStart.Z);
                            dziurka2.Radius = 2.5;
                            dziurka2.Layer = ramka;
                            //dziurka2.ColorIndex = kolorramki;
                            btr.AppendEntity(dziurka2);
                            tr.AddNewlyCreatedDBObject(dziurka2, true);
                            anonyGroup.Append(dziurka2.ObjectId);
                        }
                    }
                    else
                    {
                        // Create a papier polyline 
                        using (zzd.Polyline acPoly = new zzd.Polyline())
                        {
                            zzg.Point2d p1 = new zzg.Point2d(ptStart.X, ptStart.Y);
                            zzg.Point2d p2 = new zzg.Point2d(ptStart.X + szerokosc, ptStart.Y);
                            zzg.Point2d p3 = new zzg.Point2d(ptStart.X + szerokosc, ptStart.Y + wysokosc);
                            zzg.Point2d p4 = new zzg.Point2d(ptStart.X, ptStart.Y + wysokosc);


                            acPoly.AddVertexAt(0, p1, 0, 0, 0);
                            acPoly.AddVertexAt(1, p2, 0, 0, 0);
                            acPoly.AddVertexAt(2, p3, 0, 0, 0);
                            acPoly.AddVertexAt(3, p4, 0, 0, 0);


                            acPoly.Closed = true;
                            //acPoly.ColorIndex = kolorpapier;
                            acPoly.Layer = papier;

                            // Add the new object to the block table record and the transaction
                            btr.AppendEntity(acPoly);
                            tr.AddNewlyCreatedDBObject(acPoly, true);
                            anonyGroup.Append(acPoly.ObjectId);
                        }

                        // Create a  ramka polyline 
                        using (zzd.Polyline ramkaPoly = new zzd.Polyline())
                        {
                            zzg.Point2d r1 = new zzg.Point2d(ptStart.X + margines_lewy, ptStart.Y + margines_dolny);
                            zzg.Point2d r2 = new zzg.Point2d(ptStart.X + szerokosc - margines_lewy, ptStart.Y + margines_dolny);
                            zzg.Point2d r3 = new zzg.Point2d(ptStart.X + szerokosc - margines_lewy, ptStart.Y + wysokosc - margines_gorny);
                            zzg.Point2d r4 = new zzg.Point2d(ptStart.X + margines_lewy, ptStart.Y + wysokosc - margines_gorny);



                            ramkaPoly.AddVertexAt(0, r1, 0, 0, 0);
                            ramkaPoly.AddVertexAt(1, r2, 0, 0, 0);
                            ramkaPoly.AddVertexAt(2, r3, 0, 0, 0);
                            ramkaPoly.AddVertexAt(3, r4, 0, 0, 0);

                            //ramkaPoly.ColorIndex = kolorramki;
                            ramkaPoly.Closed = true;
                            ramkaPoly.Layer = ramka;

                            // Add the new object to the block table record and the transaction
                            btr.AppendEntity(ramkaPoly);
                            tr.AddNewlyCreatedDBObject(ramkaPoly, true);
                            anonyGroup.Append(ramkaPoly.ObjectId);
                        }

                        //tworzę miejsca na dziurkacz
                        using (zzd.Circle dziurka1 = new zzd.Circle())
                        {
                            dziurka1.Center = new zzg.Point3d(ptStart.X + szerokosc / 2 - 40, ptStart.Y + wysokosc - 10, ptStart.Z);
                            dziurka1.Radius = 2.5;
                            dziurka1.Layer = ramka;
                            //dziurka1.ColorIndex = kolorramki;
                            btr.AppendEntity(dziurka1);
                            tr.AddNewlyCreatedDBObject(dziurka1, true);
                            anonyGroup.Append(dziurka1.ObjectId);
                        }
                        using (zzd.Circle dziurka2 = new zzd.Circle())
                        {
                            dziurka2.Center = new zzg.Point3d(ptStart.X + szerokosc / 2 + 40, ptStart.Y + wysokosc - 10, ptStart.Z);
                            dziurka2.Radius = 2.5;
                            dziurka2.Layer = ramka;
                            //dziurka2.ColorIndex = kolorramki;
                            btr.AppendEntity(dziurka2);
                            tr.AddNewlyCreatedDBObject(dziurka2, true);
                            anonyGroup.Append(dziurka2.ObjectId);
                        }
                    }

                    //znacznik 210 mm

                    if (wysokosc != 210 & szerokosc > 210)
                    {
                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + 210, ptStart.Y + margines_dolny, ptStart.Z), new zzg.Point3d(ptStart.X + 210, ptStart.Y + margines_dolny + 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }

                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + 210, ptStart.Y + wysokosc - margines_gorny, ptStart.Z), new zzg.Point3d(ptStart.X + 210, ptStart.Y + wysokosc - margines_gorny - 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }
                    }

                    if (szerokosc >= 594)
                    {
                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 210 + margines_lewy, ptStart.Y + margines_dolny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 210 + margines_lewy, ptStart.Y + margines_dolny + 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }

                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 210 + margines_lewy, ptStart.Y + wysokosc - margines_gorny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 210 + margines_lewy, ptStart.Y + wysokosc - margines_gorny - 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }
                    }

                    if (szerokosc >= 841)
                    {
                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 2 * (210 - margines_lewy), ptStart.Y + margines_dolny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 2 * (210 - margines_lewy), ptStart.Y + margines_dolny + 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }

                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 2 * (210 - margines_lewy), ptStart.Y + wysokosc - margines_gorny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 2 * (210 - margines_lewy), ptStart.Y + wysokosc - margines_gorny - 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }
                    }

                    if (szerokosc >= 1189)
                    {
                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 3 * (210 - margines_lewy), ptStart.Y + margines_dolny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 3 * (210 - margines_lewy), ptStart.Y + margines_dolny + 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }

                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 3 * (210 - margines_lewy), ptStart.Y + wysokosc - margines_gorny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 3 * (210 - margines_lewy), ptStart.Y + wysokosc - margines_gorny - 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }

                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 4 * (210 - margines_lewy), ptStart.Y + margines_dolny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 4 * (210 - margines_lewy), ptStart.Y + margines_dolny + 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }

                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + szerokosc - 4 * (210 - margines_lewy), ptStart.Y + wysokosc - margines_gorny, ptStart.Z), new zzg.Point3d(ptStart.X + szerokosc - 4 * (210 - margines_lewy), ptStart.Y + wysokosc - margines_gorny - 1.0, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }
                    }

                    if (wysokosc >= 841)
                    {
                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + margines_lewy, ptStart.Y + 594, ptStart.Z), new zzg.Point3d(ptStart.X + margines_lewy + 1.5, ptStart.Y + 594, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }
                    }

                    if (wysokosc >= 1189)
                    {
                        using (zzd.Line karta = new zzd.Line(new zzg.Point3d(ptStart.X + margines_lewy, ptStart.Y + 841, ptStart.Z), new zzg.Point3d(ptStart.X + margines_lewy + 1.5, ptStart.Y + 841, ptStart.Z)))
                        {
                            karta.Layer = ramka;
                            //karta.ColorIndex = kolorramki;
                            btr.AppendEntity(karta);
                            tr.AddNewlyCreatedDBObject(karta, true);
                            anonyGroup.Append(karta.ObjectId);
                        }
                    }

                    foreach (zzd.ObjectId id in anonyGroup.GetAllEntityIds())
                    {
                        zzd.Entity entity = (zzd.Entity)tr.GetObject(id, zzd.OpenMode.ForWrite);
                        jigger2.AddEntity(entity);
                    }
                         
                    zze.PromptResult jigRes;
                    jigRes = ed.Drag(jigger2);

                    if (jigRes.Status == zze.PromptStatus.OK)
                    {

                        jigger2.TransformEntities();
                        tr.AddNewlyCreatedDBObject(anonyGroup, true);
                        tr.Commit();
                    }
                    else if ((jigRes.Status == zze.PromptStatus.Cancel))
                    {
                        return;
                    }
                    else  tr.Abort();                    
                    
                }
            }               
            
        }

    }
}
