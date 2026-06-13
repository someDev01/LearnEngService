import styles from '../opportunites/opportunies.module.css';
import OpportunityCard from '../oppotrunity_card/OpportunityCard';

function Opportunites(){
    return(
        <div className={styles.opportunites}>
            <OpportunityCard title="Короткие видео" type="videos" description="Смотри сцены из видео без скуки"/>
            <OpportunityCard title="Субтитры" type="subs" description="Кликай на слова и сразу понимай их значение"/>
            <OpportunityCard title="Словарь" type="disc" description="Сохраняй новые слова и учи в удобном формате"/>
            <OpportunityCard title="Тренировка" description="Закрепляй знания и постоянно тренируйся"/>
        </div>
    )
}

export default Opportunites;