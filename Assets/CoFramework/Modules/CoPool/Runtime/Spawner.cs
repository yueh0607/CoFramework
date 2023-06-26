using System.Collections.Generic;

public interface ISpawner<T>
{


    public IEnumerator<T> OnCreate();

    public void OnDestroy(T item);

    public void OnGet(T item);

    public void OnSet(T item);



}


