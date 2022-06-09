using System.Collections;
using UnityEngine;

public class ReturnCoroutine<T> {
    IEnumerator target;
    T result;
    public ReturnCoroutine(IEnumerator target) {
        this.target = target;
    }
    public IEnumerator Routine() {

        while (target.MoveNext()) {
            if (target.Current is T) {
                result = (T)target.Current;
            }
            yield return target.Current;
        }

    }

    public T GetResult() {
        return result;
    }
}