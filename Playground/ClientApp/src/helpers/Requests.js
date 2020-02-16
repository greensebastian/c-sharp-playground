async function call(path, data, method = "POST", parameters) {
  let url = path;
  if (parameters) {
    url = appendParameters(url, parameters);
  }
  try {
    const response = await fetch(url, {
      method: method,
      mode: "cors",
      cache: "no-cache",
      credentials: "same-origin",
      headers: {
        "Content-Type": "application/json"
      },
      redirect: "follow",
      referrerPolicy: "no-referrer",
      body: data
    });
    let output = {};
    if (response.status === 200){
      try {
        output = await response.json();
      } catch (ex){
        output.jsonException = ex;
      }
    }
    output.statusCode = response.status;
    return output;
  } catch (ex) {
    return { exception: ex };
  }
}

function appendParameters(path, parameters) {
  if (parameters) {
    if (path.indexOf("?") < 0) path += "?";

    parameters.forEach(element => {
      let lastCharacter = path[path.length - 1];
      if (lastCharacter !== "?" && lastCharacter !== "&") path += "&";
      path += element.key + "=" + element.value;
    });
  }
}

function parameter(key, value) {
  return { key: key, value: value };
}

export { call, parameter };
