using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// THIS IS A PUBLIC REPOSITORY!
// don't reference internal classes.

public class ControllerMain : MonoBehaviour {

  public Text          output;
  public RectTransform buttonContainer;
  public Button        exampleButton;
  public Button        clearButton;
  public Text          fpsText;

  private int          targetFramerate    = 120;
  private float        fpsMonitorInterval = 0.25f; // seconds

  void Start() {

    addButton("Vector Math", onClickCompareVectorMath);
    addButton("list foreach()"  , onClickForeachList1);
    addButton("list for()"      , onClickForeachList2);
    addButton("array foreach()" , onClickForeachArray1);
    addButton("array for()"     , onClickForeachArray2);

    clearButton.onClick.AddListener(onClickClear);

    outputClear();
    outputLine ("setting target framerate to " + targetFramerate);
    Application.targetFrameRate = targetFramerate;

    StartCoroutine(crFPS());
  }

  IEnumerator crFPS() {
    int   frameCount = Time.frameCount;
    float realtime   = Time.realtimeSinceStartup;

    while (true) {
      yield return new WaitForSecondsRealtime(fpsMonitorInterval);
      int   dfc = Time.frameCount - frameCount;
      float dt  = Time.realtimeSinceStartup - realtime;
      float fps = (float)dfc / dt;
      fpsText.text = "fps: " + fps.ToString("000");
      frameCount = Time.frameCount;
      realtime   = Time.realtimeSinceStartup;
    }
  }

  void addButton(string label, UnityAction onClick) {
    Button btn = Instantiate<Button>(exampleButton);
    btn.GetComponentInChildren<Text>().text = label;
    btn.onClick.AddListener(onClick);
    btn.transform.SetParent(buttonContainer);
    btn.transform.localPosition = Vector3.zero;
    btn.transform.localScale    = Vector3.one;
  }

  void onClickClear() {
    outputClear();
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

    int numIters = 20000000;

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
    outputLine("");

    /*
    method 1: 27581670
    method 2: 27299652
    method 3: 63913008
    method 4: 61806856
    */

    yield break;
  }

  void onClickForeachList1() {
    StartCoroutine(crTestForeachList<myClass>(1));
  }

  void onClickForeachList2() {
    StartCoroutine(crTestForeachList<myClass>(2));
  }

  void onClickForeachArray1() {
    StartCoroutine(crTestForeachArray<myClass>(1));
  }

  void onClickForeachArray2() {
    StartCoroutine(crTestForeachArray<myClass>(2));
  }

  private int forNumVals = 1000000;

  IEnumerator crTestForeachList<T>(int method) where T:new() {

    float numSecs   = 5f;

    string methodName = (method == 1 ? "foreach" : "for");
    outputLine(methodName + " test on List<" + typeof(T).Name + ">[" + forNumVals + "], each frame for " + numSecs.ToString("0") + " s");
    yield return null;

    List<T>values = new List<T>();

    for (int n = 0; n < forNumVals; ++n) {
      values.Add(new T());
    }

    float timeStop;

    int fc1a = 1;
    int fc1b = 2;
    int fc2a = 1;
    int fc2b = 2;

    if (method == 1) {
      outputString(".");
      yield return null;

      timeStop = Time.realtimeSinceStartup + numSecs;
      fc1a = Time.frameCount;
      while (Time.realtimeSinceStartup < timeStop) {
        foreach(T val in values) {
          foreachCore<T>(val);
        }
        yield return null;
      }
      fc1b = Time.frameCount;
    }

    if (method == 2) {

      outputString(".");
      yield return null;

      timeStop = Time.realtimeSinceStartup + numSecs;
      fc2a = Time.frameCount;
      while (Time.realtimeSinceStartup < timeStop) {
        for (int n = 0; n < forNumVals; ++n) {
          foreachCore<T>(values[n]);
        }
        yield return null;
      }
      fc2b = Time.frameCount;
    }

    int fps1 = (int)((float)(fc1b - fc1a) / numSecs);
    int fps2 = (int)((float)(fc2b - fc2a) / numSecs);

    if (method == 1) {
      outputLine("foreach: " + fps1 + " fps");
    }

    if (method == 2) {
      outputLine("for:     " + fps2 + " fps");
    }

    outputLine("");

    yield break;
  }

  IEnumerator crTestForeachArray<T>(int method) where T:new() {

    float numSecs   = 5f;

    string methodName = (method == 1 ? "foreach" : "for");
    outputLine(methodName + " test on Array<" + typeof(T).Name + ">[" + forNumVals + "], each frame for " + numSecs.ToString("0") + " s");
    yield return null;

    T[] values = new T[forNumVals];

    for (int n = 0; n < forNumVals; ++n) {
      values[n] = new T();
    }

    float timeStop;

    int fc1a = 1;
    int fc1b = 2;
    int fc2a = 1;
    int fc2b = 2;

    if (method == 1) {
      outputString(".");
      yield return null;

      timeStop = Time.realtimeSinceStartup + numSecs;
      fc1a = Time.frameCount;
      while (Time.realtimeSinceStartup < timeStop) {
        foreach(T val in values) {
          foreachCore<T>(val);
        }
        yield return null;
      }
      fc1b = Time.frameCount;
    }

    if (method == 2) {

      outputString(".");
      yield return null;

      timeStop = Time.realtimeSinceStartup + numSecs;
      fc2a = Time.frameCount;
      while (Time.realtimeSinceStartup < timeStop) {
        for (int n = 0; n < forNumVals; ++n) {
          foreachCore<T>(values[n]);
        }
        yield return null;
      }
      fc2b = Time.frameCount;
    }

    int fps1 = (int)((float)(fc1b - fc1a) / numSecs);
    int fps2 = (int)((float)(fc2b - fc2a) / numSecs);

    outputLine("foreach vs. for, " + forNumVals + " values each frame for " + numSecs.ToString("0") + " seconds");
    if (method == 1) {
      outputLine("foreach: " + fps1 + " fps");
    }

    if (method == 2) {
      outputLine("for:     " + fps2 + " fps");
    }

    outputLine("");

    yield break;
  }
  void foreachCore<T>(T obj) where T:new() {
    if (obj == null) {
      Debug.Log("unexpected");
    }
  }

  class myClass {
    public int   val1 = 1;
    public float val2 = 2.3f;
  }

}
