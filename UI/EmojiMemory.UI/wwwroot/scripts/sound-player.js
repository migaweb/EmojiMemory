
window.soundPlayer = (function () {
  const sounds = {};
  const fileNames = ["Flip", "Match", "Mismatch", "Win"];

  // Preload each sound into a dictionary
  fileNames.forEach(name => {
    const audio = new Audio(`/sounds/${name}.wav`);
    audio.load(); // Preload
    sounds[name] = audio;
  });

  return {
    play: function (name) {
      if (sounds[name]) {
        const audioClone = sounds[name].cloneNode(); // Allow overlapping playback
        audioClone.play();
      } else {
        console.warn(`Sound ${name} not found.`);
      }
    }
  };
})();
