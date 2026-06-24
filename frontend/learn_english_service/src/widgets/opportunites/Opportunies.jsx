import { useState } from 'react';
import styles from './opportunies.module.css';
import OpportunityOption from '../../ui/oppotrunity_option/OpportunityOption';

const options = [
    {
            title: 'Короткие видео',
            content: 'Смотри сцены из видео без скуки',
            type: 'clip',
            img: <img src='/opp/videos.png' alt=''/>
        },
        {
            title: 'Субтитры',
            content: 'Кликай на слова и сразу понимай их значение',
            type: 'sub',
            img: <img src='/opp/subs.png' alt=''/>
        },
        {
            title: 'Словарь',
            content: 'Сохраняй новые слова и учи в удобном формате',
            type: 'dict',
            img: <img src='/opp/dict.png' alt=''/>
        },
        {
            title: 'Тренировка',
            content: 'Закрепляй знания и постоянно тренируйся',
            type: 'train',
            img: <img src='/opp/train.png' alt=''/>
        }
];

function Opportunites(){

    const [openIndex, setOpenIndex] = useState(null);

    return(
        <div className={styles.opportunites}>
            {options.map((op, index) => (
                <OpportunityOption
                    key={index}
                    index={index}
                    openIndex={openIndex}
                    title={op.title}
                    type={op.type}
                    imgTag={op.img}
                    content={op.content}
                    onClick={() => setOpenIndex(openIndex === index ? null : index)}
                />
            ))}
        </div>
    )
}

export default Opportunites;