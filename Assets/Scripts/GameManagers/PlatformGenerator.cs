
using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour
{
	public GameObject blockPrefab;
	public GameObject platformPrefab;
	private const float SPAWN_TIME = 4f;
	private float spawnTimer = 0f;
	private float startTime;
	private bool started = false;
	private const int MAX_BLOCKS = 4;
	private const float MAX_BLOCK_X = 6f;
	private const float BLOCK_OFFSET_X = 1.2f;
//	private EdgeCollider2D edgeCollider;
	private GameObject[] blocks;


	public void Start()
	{
		this.blocks = new GameObject[MAX_BLOCKS];
		Time.timeScale = 0f;
		startTime = Time.realtimeSinceStartup;
	}
	
	public void Update()
	{
		// Start
		if(!started && (Time.realtimeSinceStartup - startTime) >= 1f) {
			Time.timeScale = 1f;
			started = true;
		}
		spawnTimer -= Time.deltaTime;
		
		if(spawnTimer <= 0f) {
			SpawnPlatform();			
			spawnTimer = SPAWN_TIME + Random.Range(0f, 1f);
		}
	}
	
	public void SpawnPlatform()
	{
//		GameObject platform = new GameObject("Platform");

		int blockCount = Random.Range(1, MAX_BLOCKS + 1);
		Vector3 blockPosition = new Vector3(Random.Range(-MAX_BLOCK_X, MAX_BLOCK_X), 10f);
		for(int i=0; i < blockCount; i++) {
			if(blockPosition.x <= MAX_BLOCK_X) {
				this.blocks[i] = (GameObject)GameObject.Instantiate(blockPrefab, blockPosition, Quaternion.identity);
				blockPosition.x += BLOCK_OFFSET_X;
			}
		}

		Vector3 platformPosition = this.blocks[0].transform.position;
		GameObject platform = (GameObject)GameObject.Instantiate(platformPrefab, platformPosition, Quaternion.identity);

		// Add edge collider with length based on block count
		platform.AddComponent<EdgeCollider2D>();
		Vector2 startPoint = transform.position + new Vector3(-BLOCK_OFFSET_X / 2f, 1.2f / 2f);
		Vector2 endPoint = startPoint + new Vector2(BLOCK_OFFSET_X * blockCount, 0);
		Vector2[] points = new Vector2[]{startPoint, endPoint};
		platform.GetComponent<EdgeCollider2D>().points = points;
		// reset blocks array
		this.blocks = new GameObject[MAX_BLOCKS];
	}
}
