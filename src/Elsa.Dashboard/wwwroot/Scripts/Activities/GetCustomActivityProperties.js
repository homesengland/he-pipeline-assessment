//API Call to get Properties of Custom Activity child Elements.  I.e. A Question Screen's Question Property.
//Need to translate into classes that can be injested by Elsa.
//Set up Common Models Folder between here, and SRC?

//export async function GetCustomActivityProperties(serverUrl) {

//  serverUrl = serverUrl += '/activities/properties'
//  console.log("RequestUrl", serverUrl);
//  const headers = new Headers();
//  headers.append("Content-Disposition", "inline");
//  const request = new Request(serverUrl, { method: 'GET', headers: headers });
//  var properties = {};
//  fetch(request)
//    .then((response) => response.json())
//    .then((data) => console.log("Data",data))
//    .then((data) => properties = data)
//    .catch((e) => console.log(e));

//  console.log("custom properties", properties)

//  return properties;
//}

export async function GetCustomActivityProperties(serverUrl) {

  serverUrl = serverUrl += '/activities/properties'
  const headers = new Headers();
  headers.append("Content-Disposition", "inline");
  const request = new Request(serverUrl, { method: 'GET', headers: headers });
  //const response = await fetch(request);
  const response = await FetchRetry(request, 10000, 5);
  const json = response.json();
  return json;
}

function wait(delay) {
  return new Promise((resolve) => setTimeout(resolve, delay));
}

async function FetchRetry(url, delay, tries, fetchOptions = {}) {
  let jsonResponse = "";
  function onError(err) {
    triesLeft = tries - 1;
    if (!triesLeft) {
      throw err;
    }
    return wait(delay).then(() => fetchRetry(url, delay, triesLeft, fetchOptions));
  }
  jsonResponse = fetch(url, fetchOptions).catch(onError);
  return jsonResponse
}
