window.soundPlayer = {
  play: function (soundName) {
    const audio = new Audio(`/sounds/${soundName}.wav`);
    audio.play();
  }
};
