**⚠️ Code no longer works as website's UI has changed ⚠️**

**ℹ️ Feel free to submit a pull request ℹ️**

# InfiniteCraftAutomation

Saw https://neal.fun/infinite-craft/.

Automated it in C# because I can. Find below the javascript version if you just want to quickly do this in a browser. The minified version is just for easy copy-pasting into the address bar for mobile devices.

## Javascript version

```
let maxElementReachedForElement = {};
let totalElements = 0;
let firstElement = 0;
let secondElement = 0;

setInterval(function() {
		document.getElementsByClassName('mobile-item')[firstElement].getElementsByClassName('item')[0].click();
		document.getElementsByClassName('mobile-item')[secondElement].getElementsByClassName('item')[0].click();
		totalElements = document.getElementsByClassName('mobile-item').length;

		secondElement = (secondElement + 1) % totalElements;

		if (secondElement == 0) {
			maxElementReachedForElement[firstElement] = totalElements;	
			if (Object.keys(maxElementReachedForElement).some(item => maxElementReachedForElement[item] < totalElements)) {
				let prevStart = Object.keys(maxElementReachedForElement).find(item => maxElementReachedForElement[item] < totalElements);
				firstElement = prevStart;
				secondElement = maxElementReachedForElement[prevStart]; // Start from previous end
			} else {
				firstElement = (firstElement + 1) % totalElements;
				secondElement = firstElement; // No need to repeat the previous combinations.
			}
		}

		document.title = firstElement + '+' + secondElement + '|' + totalElements;
}, 500); // TODO : Find a way other than delay
```

### Minified Javascript

Note : Make sure the "javascript:" at the begining is actually present in the address bar after you paste it. 
```
javascript:let maxElementReachedForElement={},totalElements=0,firstElement=0,secondElement=0;setInterval(function(){if(document.getElementsByClassName("mobile-item")[firstElement].getElementsByClassName("item")[0].click(),document.getElementsByClassName("mobile-item")[secondElement].getElementsByClassName("item")[0].click(),0==(secondElement=(secondElement+1)%(totalElements=document.getElementsByClassName("mobile-item").length))){if(maxElementReachedForElement[firstElement]=totalElements,Object.keys(maxElementReachedForElement).some(e=>maxElementReachedForElement[e]<totalElements)){let e=Object.keys(maxElementReachedForElement).find(e=>maxElementReachedForElement[e]<totalElements);firstElement=e,secondElement=maxElementReachedForElement[e]}else secondElement=firstElement=(firstElement+1)%totalElements}document.title=firstElement+"+"+secondElement+"|"+totalElements},500);
```
