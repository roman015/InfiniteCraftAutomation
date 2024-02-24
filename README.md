# InfiniteCraftAutomation

Saw https://neal.fun/infinite-craft/.

Automated it in C# because I can. Find below the javascript version if you just want to quickly do this in a browser. The minified version is just for easy copy-pasting into the address bar for mobile devices.

## Javascript version

```
let firstElement = 0;
let secondElement = 1;

setInterval(function() {
    let mobileItems = document.getElementsByClassName('mobile-item');
    if (mobileItems.length > 0) {
        mobileItems[firstElement].getElementsByClassName('item')[0].click();
        mobileItems[secondElement].getElementsByClassName('item')[0].click();
        let totalElements = mobileItems.length;
        firstElement = (firstElement + 1) % totalElements;
        secondElement = (secondElement + 1) % totalElements;
        document.title = firstElement + '+' + secondElement + '|' + totalElements;
    }
}, 500);

```

### Minified Javascript

Note : Make sure the "javascript:" at the begining is actually present in the address bar after you paste it. 
```
javascript:let maxElementReachedForElement={},totalElements=0,firstElement=0,secondElement=0;setInterval(function(){if(document.getElementsByClassName("mobile-item")[firstElement].getElementsByClassName("item")[0].click(),document.getElementsByClassName("mobile-item")[secondElement].getElementsByClassName("item")[0].click(),0==(secondElement=(secondElement+1)%(totalElements=document.getElementsByClassName("mobile-item").length))){if(maxElementReachedForElement[firstElement]=totalElements,Object.keys(maxElementReachedForElement).some(e=>maxElementReachedForElement[e]<totalElements)){let e=Object.keys(maxElementReachedForElement).find(e=>maxElementReachedForElement[e]<totalElements);firstElement=e,secondElement=maxElementReachedForElement[e]}else secondElement=firstElement=(firstElement+1)%totalElements}document.title=firstElement+"+"+secondElement+"|"+totalElements},500);
```
