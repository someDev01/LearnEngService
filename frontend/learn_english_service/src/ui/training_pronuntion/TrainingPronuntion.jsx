import { Volume2Icon } from "lucide-react";
import { useSpeech } from "../../utils/speesh/useSpeesh";
import styles from '../training_pronuntion/training_pronuntion.module.css';

function TrainingPronuntion({target}){

    const {speak, toggleRate, rate} = useSpeech(target); 

    return(
        <section className={styles.pronuntion}>
            <div className={styles.buttons}>
                <button className={styles.pronuntion_button} type="button" onClick={speak}><Volume2Icon size={28} color="#ff7124"/></button>
                <button className={styles.rate_button} type="button" onClick={toggleRate}>x{rate}</button>
            </div>
            <p>Нажми чтобы услышать</p>
        </section>
    )
}

export default TrainingPronuntion;