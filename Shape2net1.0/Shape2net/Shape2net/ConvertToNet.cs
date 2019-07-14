using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;



namespace Shape2net
{
    class ConvertToNet
    {
        private string ShapeFilePath = "";
        private string NetFilePath = "";
        private string ShapeFileName = "";

        private IWorkspace OpenShapfileWorkspace(string ShapeFilePath)
        {
            IWorkspace ws = null;
            IWorkspaceFactory wsf = new ShapefileWorkspaceFactoryClass();
            ws = wsf.OpenFromFile(this.ShapeFilePath, 0);
            return ws;
        }

        public  ConvertToNet(string shapfilePath, string shapeFileName,string netfilePath) //构造函数
        {
            this.ShapeFilePath = shapfilePath;
            this.ShapeFileName = shapeFileName;
            this.NetFilePath = netfilePath;
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

        }

        public void SaveNetFile(string NetFilePath) 
        {
            IWorkspace pWorkspace = OpenShapfileWorkspace(this.ShapeFilePath);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(ShapeFileName);
            IQueryFilter queryFilter = new QueryFilterClass();
            IFeatureCursor pFeatureCursor = pFeatureClass.Update(queryFilter,false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            
            String CountVertices = pFeatureClass.FeatureCount(queryFilter).ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("*Vertices ");
            sb.Append(CountVertices);
            String header = sb.ToString();
            System.IO.StreamWriter file = new System.IO.StreamWriter(NetFilePath,true);
            file.WriteLine(header);
            file.WriteLine("*Edgeslist");

            while (pFeature != null)
            {
                pSpatialFilter.Geometry = pFeature.ShapeCopy;
                pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;
                String edges = (pFeature.OID + 1).ToString();
                IFeatureCursor pFCursorInsects;
                IFeature pFInsects;
                pFCursorInsects = pFeatureClass.Search(pSpatialFilter,false);
                pFInsects = pFCursorInsects.NextFeature();
                while(pFInsects != null)
                {
                    if (pFInsects.OID > pFeature.OID)
                    {
                        edges = edges + " " + (pFInsects.OID + 1).ToString();

                    }
                    pFInsects = pFCursorInsects.NextFeature();
                    
                }                                    
                file.WriteLine(edges);
                pFeature = pFeatureCursor.NextFeature();
                
            }
            file.Close();
           
        }

    }
}
