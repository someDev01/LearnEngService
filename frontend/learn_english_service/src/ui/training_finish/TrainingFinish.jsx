import ButtonRestart from '../button_restart/ButtonRestart';
import styles from '../training_finish/training_finish.module.css';

function TrainingFinish({countCorrect, questions, onClick}){
    return(
        <div className={styles.training_finish}>
            <p>Тренировка окончена</p>
            <p>{countCorrect} / {questions.length}</p>
            <ButtonRestart onClick={onClick}/>
        </div>
    )
}

export default TrainingFinish;