import { useState } from 'react';
import styles from './opportunies.module.css';
import OpportunityOption from '../../ui/oppotrunity_option/OpportunityOption';

const options = [
    {
            title: 'Короткие видео',
            content: 'Изучай английский с помощью коротких YouTube-видео. Находи интересующие тебя видео и выбирай контент по уровню сложности — от начального до продвинутого. Это поможет изучать английский на материалах, которые подходят именно тебе',
            type: 'clip',
            img: <img src='/opp/videos.png' alt=''/>
        },
        {
            title: 'Субтитры',
            content: 'Получай перевод слов прямо во время просмотра, сохраняй их в один клик и сразу замечай уже изученную лексику благодаря автоматическомй подсветке слов',
            type: 'sub',
            img: <img src='/opp/subs.png' alt=''/>
        },
        {
            title: 'Словарь',
            content: 'Создавай личный словарь из слов, фразовых глаголов и устойчивых выражений. Для каждой заметки сохраняются переводы, примеры, источник, отуда было изучено слово',
            type: 'dict',
            img: <img src='/opp/dict.png' alt=''/>
        },
        {
            title: 'Тренировка',
            content: 'Закрепляй слова с помощью интерактивных упражнений. Тренировка строится на реальных примерах использования, а слова которые вызывают больше трудностей, автоматически повторяются чаще',
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