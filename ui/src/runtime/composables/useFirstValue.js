export default function() {
  function compute({ data } = {}) {
    console.log(data);
    return data[0];
  }

  return {
    compute
  };
}
