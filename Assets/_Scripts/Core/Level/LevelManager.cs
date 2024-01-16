using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>, IGameState
{
    [Header("DEPENDENCIES")]
    public Level level;
    public Collider innerCol;
    public Collider outterCol;
    public CubePointsData pointsData;
    public GameObject[] environments;

    [Header("DEBUG")]
    public GameObject cubeParent;
    public Level currentLevel;
    public int currentCubeCount;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (environments.Length != 0)
        {
            var levelCount = GameManager.instance.currentLevel;
            
            foreach (var environment in environments)
            {
                environment.SetActive(false);
            }

            environments[(levelCount - 1) % environments.Length].SetActive(true);

            environments[(levelCount - 1) % environments.Length].transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        }

        currentLevel = level;

        currentCubeCount = currentLevel.StartCubeCount;

        InitCubes();
    }

    public void SpawnOneCube()
    {
        if (pointsData.Points.Count == 0)
        {
            return;
        }

        int rndCubeIndex = Random.Range(0, pointsData.Points.Count - 1);

        Vector3 point = pointsData.Points[rndCubeIndex];

        pointsData.Points.Remove(point);

        var cube = Instantiate(currentLevel.CubePrefab, point, currentLevel.CubePrefab.transform.rotation);

        currentCubeCount++;

        cube.transform.SetParent(cubeParent.transform);

        cube.DoSpawnAnimation();
    }

    private void InitCubes()
    {
        cubeParent = new GameObject("Cubes");

        FindAllCubePoints();

        for (int i = 0; i < currentLevel.StartCubeCount; i++)
        {
            int rndCubeIndex = Random.Range(0, pointsData.Points.Count - 1);

            Vector3 point = pointsData.Points[rndCubeIndex];

            pointsData.Points.Remove(point);

            var cube = Instantiate(currentLevel.CubePrefab, point, currentLevel.CubePrefab.transform.rotation);

            cube.transform.SetParent(cubeParent.transform);

            cube.DoSpawnAnimation();
        }

        void FindAllCubePoints()
        {
            pointsData.Points = new System.Collections.Generic.List<Vector3>();

            for (int i = 0; i < currentLevel.TotalCubeCount; i++)
            {
                Vector3 point = FindRandomPoint();

                pointsData.Points.Add(point);
            }
        }

        Vector3 FindRandomPoint()
        {
            float dist = Random.Range(0f, 1f);

            float rndTime = Random.Range(0f, 1f);

            Vector3 outterPoint = outterCol.GetComponent<OutterCollider>().GetOutterPoint(dist);

            Vector3 closestPointToInnerCol = innerCol.ClosestPoint(outterPoint);

            Vector3 rndPoint = Vector3.Lerp(outterPoint, closestPointToInnerCol, rndTime);

            return rndPoint;
        }
    }

    #region GAME STATE CALLBACKS
    public void OnLevelStart()
    {

    }

    public void OnLevelEnd()
    {

    }

    #endregion
}
