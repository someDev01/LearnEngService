import { useEffect, useRef, useState } from 'react';

export function useSpeech(word, lang = 'en-US') {
  const rateRef = useRef(1);
  const [rate, setRate] = useState(1);

  const speak = () => {
    window.speechSynthesis.cancel();
    
    const utterance = new SpeechSynthesisUtterance(word);
    utterance.lang = lang;
    utterance.rate = rateRef.current;
    utterance.pitch = 1;
    utterance.volume = 1;
    
    window.speechSynthesis.speak(utterance);
  };

  const toggleRate = () => {
    const newRate = rateRef.current === 1 ? 0.5 : rateRef.current === 0.5 ? 0.25 : 1;
    rateRef.current = newRate;
    setRate(newRate);
  };

  return {
    speak,
    toggleRate,
    rate
  };
}