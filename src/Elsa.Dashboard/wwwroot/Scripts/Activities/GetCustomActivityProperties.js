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
  const response = await fetch(request);
  const json = response.json();
  return json;
}
