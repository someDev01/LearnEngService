import styles from '../button_question_translation/button_question_translation.module.css';

function ButtonQuestionTranslation({onClick}){
    return(
        <div className={styles.showTranslation_btn} onClick={onClick}>
            <p>показать  перевод</p>
        </div>
    )
}

export default ButtonQuestionTranslation;