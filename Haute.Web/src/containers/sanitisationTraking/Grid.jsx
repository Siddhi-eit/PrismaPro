import React from 'react';
import { Card, Row } from 'reactstrap';
import ContextMenuContainer from 'containers/pages/ContextMenuContainer';
import Pagination from 'containers/pages/Pagination';
import GirdThumbListView from '../pages/sanitisationTraking/GirdThumbListView';
import { Colxx } from 'components/common/CustomBootstrap';

function collect(props) {
  return { data: props.data };
}

const Grid = ({
  items,
  allSanitisationItems,
  displayMode,
  selectedItems,
  onCheckItem,
  currentPage,
  totalPage,
  onContextMenuClick,
  onContextMenu,
  onChangePage,
}) => {
  return (
    <Row>
      <Colxx xxs="12">
        <Card>
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold text-start">
                Fusion Lab No
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                Canister Code
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold text-center">
                Date Sanitised
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold text-center">
                Created Date
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold text-center">
                Is Sanitized ?
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold text-center">
                Is Active ?
              </p>
            </div>
          </div>
        </Card>
      </Colxx>
      {items.map((sanitisation) => {
        if (displayMode === 'thumblist') {
          return (
            <GirdThumbListView
              key={sanitisation.id}
              sanitisation={sanitisation}
              isSelect={selectedItems.includes(sanitisation.id)}
              collect={collect}
              onCheckItem={onCheckItem}
            />
          );
        }
        return <></>;
      })}
      {allSanitisationItems === null ||
        (items.length === 0 && (
          <h3 className="mb-3 w-100 text-center">No Data Found</h3>
        ))}
      <Pagination
        currentPage={currentPage}
        totalPage={totalPage}
        onChangePage={(i) => onChangePage(i)}
      />
      <ContextMenuContainer
        onContextMenuClick={onContextMenuClick}
        onContextMenu={onContextMenu}
      />
    </Row>
  );
};

export default React.memo(Grid);
