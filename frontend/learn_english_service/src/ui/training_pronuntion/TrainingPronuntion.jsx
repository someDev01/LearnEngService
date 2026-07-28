import { AudioWaveformIcon, Volume2Icon } from "lucide-react";
import styles from '../training_pronuntion/training_pronuntion.module.css';
import { useSpeech } from "../../utils/speech/useSpeech";

function TrainingPronuntion({ target }) {
  const { speak, toggleRate, rate } = useSpeech(target);

  return (
    <section className={styles.pronuntion}>
      <div className={styles.waves_row}>
        <div className={styles.wave_group}>
          
          <AudioWaveformIcon size={18} />
          <AudioWaveformIcon size={26} />
          <AudioWaveformIcon size={34} />
          
        </div>

        <button className={styles.pronuntion_button} type="button" onClick={speak}>
          <Volume2Icon size={28} color="#fd9a3e" />
        </button>

        <div className={styles.wave_group}>
          <AudioWaveformIcon size={34} />
          <AudioWaveformIcon size={26} />
          <AudioWaveformIcon size={18} />
        </div>
      </div>

      <button className={styles.rate_button} type="button" onClick={toggleRate}>
        x{rate}
      </button>

      <p>Нажми чтобы услышать</p>
    </section>
  );
}

export default TrainingPronuntion;