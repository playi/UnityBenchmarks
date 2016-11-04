using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerMain : MonoBehaviour {


  public Text   output;
  public Button btnCompareVectorMath;

  void Start() {

    btnCompareVectorMath.onClick.AddListener(onClickCompareVectorMath);

    outputClear();
    outputLine ("howdy");
  }

  void outputClear() {
    output.text = "";
  }

  void outputLine(string line) {
    if (output.text != "") {
      line = "\n" + line;
    }

    outputString(line);
  }

  void outputString(string s) {
    output.text += s;
  }


  void onClickCompareVectorMath() {
    StartCoroutine(crCompareVectorMath());
  }

  IEnumerator crCompareVectorMath() {
    outputLine("starting test CompareVectorMath");
    yield return null;

    int numIters = 30000000;

    float[] scalars = new float[numIters];
    for (int n = 0; n < numIters; ++n) {
      scalars[n] = Random.Range(1f, 2f);
    }

    Vector3 vecStart = new Vector3(1f, 2f, 3f);

    Vector3 vec;
    float   s;

    outputString(".");
    yield return null;

    vec = vecStart;
    float t1a = Time.realtimeSinceStartup;
    for (int n = 0; n < numIters; ++n) {
      s    = scalars[n];
      vec *= s;
    }
    float t1b = Time.realtimeSinceStartup;

    outputString(".");
    yield return null;

    vec = vecStart;
    float t2a = Time.realtimeSinceStartup;
    for (int n = 0; n < numIters; ++n) {
      s    = scalars[n];
      vec  = vec * s;
    }
    float t2b = Time.realtimeSinceStartup;

    outputString(".");
    yield return null;

    vec = vecStart;
    float t3a = Time.realtimeSinceStartup;
    for (int n = 0; n < numIters; ++n) {
      s     = scalars[n];
      vec.x = vec.x * s;
      vec.y = vec.y * s;
      vec.z = vec.z * s;
    }
    float t3b = Time.realtimeSinceStartup;


    outputString(".");
    yield return null;

    vec = vecStart;
    float t4a = Time.realtimeSinceStartup;
    for (int n = 0; n < numIters; ++n) {
      s      = scalars[n];
      vec.x *= s;
      vec.y *= s;
      vec.z *= s;
    }
    float t4b = Time.realtimeSinceStartup;

    float t1 = t1b - t1a;
    float t2 = t2b - t2a;
    float t3 = t3b - t3a;
    float t4 = t4b - t4a;

    float opsPerS1 = numIters / t1;
    float opsPerS2 = numIters / t2;
    float opsPerS3 = numIters / t3;
    float opsPerS4 = numIters / t4;

    outputLine("operations per second, with " + numIters + " iterations.");
    outputLine("method 1: " + (int)opsPerS1);
    outputLine("method 2: " + (int)opsPerS2);
    outputLine("method 3: " + (int)opsPerS3);
    outputLine("method 4: " + (int)opsPerS4);

    /*
    method 1: 27581670
    method 2: 27299652
    method 3: 63913008
    method 4: 61806856
    */

  }

}
